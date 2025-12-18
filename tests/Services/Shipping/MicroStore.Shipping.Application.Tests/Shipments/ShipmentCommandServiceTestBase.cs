using Microsoft.Extensions.DependencyInjection;
using MicroStore.Shipping.Application.Abstraction.Common;
using MicroStore.Shipping.Application.Abstraction.Models;
using MicroStore.Shipping.Application.Tests.Fakes;
using MicroStore.Shipping.Domain.Const;
using MicroStore.Shipping.Domain.Entities;
using MicroStore.Shipping.Domain.ValueObjects;
namespace MicroStore.Shipping.Application.Tests.Shipments
{
    public class ShipmentCommandServiceTestBase : BaseTestFixture
    {

        protected Task<Shipment> CreateFakeShipment()
        {
            return WithUnitOfWork((sp) =>
            {
                var respository = sp.GetRequiredService<IShipmentRepository>();
                return respository.InsertAsync(new Shipment(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), Address.Empty));
            });
        }

        protected Task<Shipment> CreateFakeFullfilledShipment() { 
        
           return WithUnitOfWork((sp) =>
            {
                var respository = sp.GetRequiredService<IShipmentRepository>();
                return respository.InsertAsync(new Shipment(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(),Guid.NewGuid().ToString(), Address.Empty)
                {
                    Status = ShipmentStatus.Fullfilled,
                    ShipmentExternalId = Guid.NewGuid().ToString(),

                    SystemName = FakeConst.ActiveSystem
                });
            });
        }
        protected PackageModel GeneratePackageModel()
        {
            return new PackageModel
            {
                Dimension = new DimensionModel
                {
                    Height = 5,
                    Width = 5,
                    Length = 5,
                    Unit = StandardDimensionUnit.Inch
                },
                
                Weight = new WeightModel
                {
                    Value = 5,
                    Unit = StandardWeightUnit.Gram
                }
            };
        }
        protected ShipmentModel GenerateShipmentModel()
        {
            return new ShipmentModel
            {
                OrderId = Guid.NewGuid().ToString(),
                OrderNumber = Guid.NewGuid().ToString(),
                UserId = Guid.NewGuid().ToString(),
                Address = new AddressModel
                {
                    CountryCode = "US",
                    State = "CA",
                    City = "San Jose",
                    AddressLine1 = "525 S Winchester Blvd",
                    AddressLine2 = "525 S Winchester Blvd",
                    Name = "Jane Doe",
                    Phone = "8143511258",
                    PostalCode = "95128",
                    Zip = "90241"
                },

                Items = new List<ShipmentItemModel>
                {
                    new ShipmentItemModel
                    {
                        Name = Guid.NewGuid().ToString(),
                        Sku  =Guid.NewGuid().ToString(),
                        ProductId= Guid.NewGuid().ToString(),
                        Thumbnail = Guid.NewGuid().ToString(),
                        Quantity = 5,
                        Dimension =new DimensionModel
                        {
                            Height = 5,
                            Width = 5,
                            Length = 5,
                            Unit = "inch"
                        },
                        Weight = new WeightModel
                        {
                            Value = 5,
                            Unit = "gram",
                        },
                        UnitPrice = 50
                    }
                }

            };
        }

    }
}
