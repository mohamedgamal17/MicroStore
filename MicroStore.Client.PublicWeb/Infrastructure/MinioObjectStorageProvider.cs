using MicroStore.Client.PublicWeb.Configuration;
using Minio;
using Minio.DataModel.Args;
using Minio.Exceptions;
using System.IO;
using System.Text;
using Volo.Abp.DependencyInjection;
namespace MicroStore.Client.PublicWeb.Infrastructure
{
    [ExposeServices(typeof(IObjectStorageProvider))]
    public class MinioObjectStorageProvider : IObjectStorageProvider  ,ITransientDependency
    {
        private readonly ApplicationSettings _applicationSettings;
        public MinioObjectStorageProvider(ApplicationSettings applicationSettings)
        {
            _applicationSettings = applicationSettings;
        }

        public async Task<Stream?> GetAsync(string objectName, CancellationToken cancellationToken = default)
        {
            var client = GetMinioClient();

            MemoryStream? memoryStream = new MemoryStream();

            var args = new GetObjectArgs()
                .WithBucket(_applicationSettings.S3ObjectProvider.BucketName)
                .WithObject(objectName)
                .WithCallbackStream((stream) =>
                {
                    if (stream != null)
                    {
                        stream.CopyTo(memoryStream);
                        memoryStream.Seek(0, SeekOrigin.Begin);
                    }
                    else
                    {
                        memoryStream = null;
                    }
                });


            await client.GetObjectAsync(args, cancellationToken);

            return memoryStream;
        }

        public Task<string> CalculatePublicReferenceUrl(string objectName, CancellationToken cancellationToken = default)
        {
            var config = GetConfiguration();

            StringBuilder str = new StringBuilder();

            if (config.WithSSL)
            {
                str.Append("https://");
            }
            else
            {
                str.Append("http://");
            }

            str.Append(config.EndPoint.EnsureEndsWith('/'));

            str.Append(config.BucketName.EnsureEndsWith('/'));

            str.Append(objectName);

            return Task.FromResult(str.ToString());
        }

        public async Task MigrateAsync(CancellationToken cancellationToken = default)
        {
        
            bool isBucketExist = await IsBucketExist(cancellationToken);

            if (!isBucketExist)
            {
                await CreateBucketAsync(cancellationToken);

                await SetBucketAnonoymusReadonlyPolicy(cancellationToken);
            }
        }

        public async Task SaveAsync(S3ObjectSaveArgs args, CancellationToken cancellationToken = default)
        {
            var client = GetMinioClient();

            var requestArgs = new PutObjectArgs()
                .WithBucket(_applicationSettings.S3ObjectProvider.BucketName)
                .WithObject(args.Name)
                .WithStreamData(args.Data)
                .WithObjectSize(args.Data.Length)
                .WithContentType(args.ContentType);


            await client.PutObjectAsync(requestArgs, cancellationToken);
        }


        private async Task<bool> IsBucketExist(CancellationToken cancellationToken)
        {
            var client = GetMinioClient();

            var args = new BucketExistsArgs()
                .WithBucket(_applicationSettings.S3ObjectProvider.BucketName);

            return await client.BucketExistsAsync(args);
        }


        private async Task CreateBucketAsync(CancellationToken cancellationToken)
        {
            var client = GetMinioClient();

            var args = new MakeBucketArgs()
                .WithBucket(_applicationSettings.S3ObjectProvider.BucketName);


            await client.MakeBucketAsync(args, cancellationToken);
        }

        private async Task SetBucketAnonoymusReadonlyPolicy(CancellationToken cancellationToken)
        {
            var config = GetConfiguration();

            var client = GetMinioClient();

            string policy = @$"
                {{
                    ""Version"": ""2012-10-17"",
                    ""Statement"": [
                        {{
                            ""Effect"": ""Allow"",
                            ""Principal"": {{
                                ""AWS"": [
                                    ""*""
                                ]
                            }},
                            ""Action"": [
                                ""s3:ListBucket"",
                                ""s3:GetBucketLocation""
                            ],
                            ""Resource"": [
                                ""arn:aws:s3:::{config.BucketName}""
                            ]
                        }},
                        {{
                            ""Effect"": ""Allow"",
                            ""Principal"": {{
                                ""AWS"": [
                                    ""*""
                                ]
                            }},
                            ""Action"": [
                                ""s3:GetObject""
                            ],
                            ""Resource"": [
                                ""arn:aws:s3:::{config.BucketName}/*""
                            ]
                        }}
                    ]
                }}
            ";

            var args = new SetPolicyArgs()
                .WithBucket(_applicationSettings.S3ObjectProvider.BucketName)
                .WithPolicy(policy);

            await client.SetPolicyAsync(args, cancellationToken);
        }


        private IMinioClient GetMinioClient()
        {
            var config = GetConfiguration();

            var minioClient = new MinioClient()
                .WithEndpoint(_applicationSettings.S3ObjectProvider.EndPoint)
                .WithCredentials(_applicationSettings.S3ObjectProvider.AccessKey, _applicationSettings.S3ObjectProvider.SecretKey)
                .WithSSL(false);

            if (_applicationSettings.S3ObjectProvider.WithSSL)
            {
                minioClient = minioClient.WithSSL();
            }

            return minioClient.Build();
        }

        private S3StorageProviderSettings GetConfiguration()
        {
            return _applicationSettings.S3ObjectProvider;
        }
    }


    public class S3ObjectSaveArgs
    {
        public string Name { get; set; }
        public Stream Data { get; set; }
        public string ContentType { get; set; }
    }
}
