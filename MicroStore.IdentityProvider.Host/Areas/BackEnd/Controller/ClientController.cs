using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.BuildingBlocks.Paging.Params;
using MicroStore.IdentityProvider.Host.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.Host.Areas.BackEnd.Models.Clients;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
namespace MicroStore.IdentityProvider.Host.Areas.BackEnd.Controller
{
    public class ClientController : BackEndController
    {

        private readonly IClientCommandService _clientCommandService;

        private readonly IClientQueryService _clientQueryService;

        private readonly IApiScopeQueryService _apiScopeQueryService;

        public ClientController(IClientCommandService clientCommandService, IClientQueryService clientQueryService, IApiScopeQueryService apiScopeQueryService)
        {
            _clientCommandService = clientCommandService;
            _clientQueryService = clientQueryService;
            _apiScopeQueryService = apiScopeQueryService;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Index(ClientListUIModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var pagingOptions = new PagingQueryParams { Length = model.PageSize, Skip= model.Skip };

            var result=  await _clientQueryService.ListAsync(pagingOptions);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Exception.Message);
            }

            model.Data = result.Value.Items;

            model.RecordsTotal = result.Value.TotalCount;

            return Json(model);
        }

     
        public IActionResult Create()
        {
            return View(new CreateClientUIModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClientUIModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var clientModel = PrepareClientModel(model);

            var result = await _clientCommandService.CreateAsync(clientModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }

            return RedirectToAction("Edit", new { id = result.Value.Id });
        }


       
        public async Task<IActionResult> Edit(int id)
        {
            var result = await _clientQueryService.GetAsync(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }
            
            var model = ObjectMapper.Map<ClientDto, EditClientUIModel>(result.Value);

            ViewBag.ApiScopes = await PrepareClientScopes(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditClientUIModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ApiScopes = await PrepareClientScopes(model);

                return View(model);
            }

            var clientModel = ObjectMapper.Map<EditClientUIModel ,ClientModel >(model);

            var result = await _clientCommandService.UpdateAsync(model.Id, clientModel);

            if (result.IsFailure)
            {
                ViewBag.ApiScopes = await PrepareClientScopes(model);

                return HandleFailureResultWithView(result,model);
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _clientCommandService.DeleteAsync(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> ListClientSecrets(int id , ClientSecretListUIModel model)
        {
            var result = await _clientQueryService.ListClientSecrets(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            model.Data = result.Value;

            model.RecordsTotal = result.Value.Count;

            return Json(model);
        }


        public IActionResult  CreateClientSecretModal(int clientId)
        {
            return PartialView("",new ClientSecretUIModel { ClientId = clientId});
        }


        [HttpPost]
        public async Task<IActionResult> CreateClientSecret(ClientSecretUIModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result =   await _clientCommandService.AddClientSecret(model.ClientId, model);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Json(result.Value);
        }


        [HttpPost]
        public async Task<IActionResult> RemoveClientSecret([FromBody] RemoveClientSecretUIModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _clientCommandService.DeleteClientSecret(model.ClientId, model.SecretId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Json(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> ListClaims(int clientId , ClientClaimListModel model)
        {
            var result = await _clientQueryService.ListClaims(clientId);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Exception.Message);
            }

            model.Data = result.Value;

            return Json(model);
        }

        public IActionResult CreateClaimModal(int clientId)
        {
            return PartialView("_Create.Claim", new ClientClaimUIModel { ClientId = clientId });
        }

        public async Task<IActionResult> EditClaimModal(int clientId, int claimId)
        {
            var result = await _clientQueryService.GetClaim(clientId, claimId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }

            var model = new ClientClaimUIModel
            {
                ClientId = result.Value.ClientId,
                ClaimId = result.Value.Id,
                Type = result.Value.Type,
                Value = result.Value.Value
            };

            return PartialView("_Edit.Claim", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClaim(ClientClaimUIModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var claimModel = new ClaimModel
            {
                Type = model.Type,
                Value = model.Value
            };

            var result = await _clientCommandService.AddClaim(model.ClientId, claimModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }
            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateClaim(ClientClaimUIModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            var claimModel = new ClaimModel
            {
                Type = model.Type,
                Value = model.Value
            };



            var result = await _clientCommandService.UpdateClaim(model.ClientId, model.ClaimId,claimModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveClaim([FromBody]RemoveClientClaimUIModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _clientCommandService.RemoveClaim(model.ClientId, model.ClaimId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return NoContent();
        }


        [HttpPost]
        public async Task<IActionResult> ListProperties(int id, ClientPropertyListModel model)
        {
            var result = await _clientQueryService.ListProperties(id);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }


            model.Data = result.Value;

            return Json(model);
        }

        public IActionResult CreatePropertyModal(int parentId)
        {
            return PartialView("_Create.Property", new PropertyUIModel { ParentId = parentId });
        }

        public async Task<IActionResult> EditPropertyModal(int parentId, int propertyId)
        {
            var result = await _clientQueryService.GetProperty(parentId, propertyId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }

            return PartialView("_Edit.Property", new PropertyUIModel
            {
                ParentId = result.Value.ClientId,
                PropertyId = result.Value.Id,
                Key = result.Value.Key,
                Value = result.Value.Value
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProperty(PropertyUIModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var propertyModel = ObjectMapper.Map<PropertyUIModel, PropertyModel>(model);

            var result = await _clientCommandService.AddProperty(model.ParentId, propertyModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProperty(PropertyUIModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var propertyModel = ObjectMapper.Map<PropertyUIModel, PropertyModel>(model);

            var result = await _clientCommandService.UpdateProperty(model.ParentId, model.PropertyId, propertyModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProperty([FromBody]RemovePropertyUIModel model)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _clientCommandService.RemoveProperty(model.ParentId, model.PropertyId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return NoContent();
        }


        private ClientModel PrepareClientModel(CreateClientUIModel model)
        {
            var clientModel = new ClientModel()
            {
                ClientId = model.ClientId,
                ClientName = model.ClientName,
                AllowedGrantTypes = new List<string>()
            };

            switch (model.Type)
            {
                case ClientType.Web :
                    clientModel.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    clientModel.RequirePkce = true;
                    clientModel.RequireClientSecret = true;     
                    break;
                case ClientType.Spa:
                    clientModel.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    clientModel.RequirePkce = true;
                    clientModel.RequireClientSecret = false;
                    break;
                case ClientType.Native:
                    clientModel.AllowedGrantTypes.AddRange(GrantTypes.Code);
                    clientModel.RequirePkce = true;
                    clientModel.RequireClientSecret = false;
                    break;
                case ClientType.Machine:
                    clientModel.AllowedGrantTypes.AddRange(GrantTypes.ClientCredentials);
                    break;
                case ClientType.Device:
                    clientModel.AllowedGrantTypes.AddRange(GrantTypes.DeviceFlow);
                    clientModel.RequireClientSecret = false;
                    clientModel.AllowOfflineAccess = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }


            return clientModel;
        }

        private async Task<List<SelectListItem>> PrepareClientScopes(EditClientUIModel model)
        {
            var result = await _apiScopeQueryService.ListAsync();

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Exception.Message);
            }

            return result.Value.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Name,
                Selected = model?.AllowedScopes?.Any(c => c == x.Name) ?? false

            }).ToList();
        }

     }
}
