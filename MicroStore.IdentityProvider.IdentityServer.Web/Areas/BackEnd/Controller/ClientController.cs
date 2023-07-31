using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using MicroStore.IdentityProvider.IdentityServer.Application.ApiScopes;
using MicroStore.IdentityProvider.IdentityServer.Application.Clients;
using MicroStore.IdentityProvider.IdentityServer.Application.Dtos;
using MicroStore.IdentityProvider.IdentityServer.Application.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models;
using MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Models.Clients;

namespace MicroStore.IdentityProvider.IdentityServer.Web.Areas.BackEnd.Controller
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
        public async Task<IActionResult> Index(ClientSearchModel model)
        {

            var pagingOptions = new ClientListQueryModel { Length = model.PageSize, Skip = model.Skip };

            var result = await _clientQueryService.ListAsync(pagingOptions);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Exception.Message);
            }

            var clientListvm = new ClientListViewModel
            {
                Data = result.Value.Items,
                Start = result.Value.Skip,
                Length = result.Value.Lenght,
                RecordsTotal = result.Value.TotalCount,
                Draw = model.Draw
            }; 

            return Json(clientListvm);
        }


        public IActionResult Create()
        {
            return View(new CreateClientModel());
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateClientModel model)
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

            var model = ObjectMapper.Map<ClientDto, EditClientModel>(result.Value);

            ViewBag.ApiScopes = await PrepareClientScopes(model);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id ,EditClientModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.ApiScopes = await PrepareClientScopes(model);

                return View(model);
            }

            var clientModel = ObjectMapper.Map<EditClientModel, ClientModel>(model);

            var result = await _clientCommandService.UpdateAsync(model.Id, clientModel);

            if (result.IsFailure)
            {
                ViewBag.ApiScopes = await PrepareClientScopes(model);

                return HandleFailureResultWithView(result, model);
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
        public async Task<IActionResult> ListClientSecrets(int clientId)
        {
            var result = await _clientQueryService.ListClientSecrets(clientId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            var viewModel = new ClientSecretListViewModel
            {
                Data = result.Value
            };

            return Json(viewModel);
        }


        public IActionResult CreateSecretModal(int clientId)
        {
            return PartialView("_Create.Secret", new CreateClientSecretModel { ClientId = clientId });
        }


        [HttpPost]
        public async Task<IActionResult> CreateSecret(CreateClientSecretModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _clientCommandService.AddClientSecret(model.ClientId, model);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Json(result.Value);
        }


        [HttpPost]
        public async Task<IActionResult> RemoveSecret([FromBody] RemoveClientSecretModel model)
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
        public async Task<IActionResult> ListClaims(int clientId)
        {
            var result = await _clientQueryService.ListClaims(clientId);

            if (result.IsFailure)
            {
                throw new InvalidOperationException(result.Exception.Message);
            }

            var viewModel = new ClientClaimListViewModel
            {
                Data = result.Value
            };

            return Json(viewModel);
        }

        public IActionResult CreateClaimModal(int clientId)
        {
            return PartialView("_Create.Claim", new CreateOrEditClientClaimModel { ClientId = clientId });
        }

        public async Task<IActionResult> EditClaimModal(int clientId, int claimId)
        {
            var result = await _clientQueryService.GetClaim(clientId, claimId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }
            var model = new CreateOrEditClientClaimModel
            {
                ClientId = result.Value.ClientId,
                ClaimId = result.Value.Id,
                Type = result.Value.Type,
                Value = result.Value.Value
            };

            return PartialView("_Edit.Claim", model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateClaim(CreateOrEditClientClaimModel model)
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
        public async Task<IActionResult> UpdateClaim(CreateOrEditClientClaimModel model)
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



            var result = await _clientCommandService.UpdateClaim(model.ClientId, model.ClaimId, claimModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Ok(result.Value);
        }

        [HttpPost]
        public async Task<IActionResult> RemoveClaim([FromBody] RemoveClientClaimModel model)
        {
            var body = Request.Body;
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
        public async Task<IActionResult> ListProperties(int parentId, ListModel model)
        {
            var result = await _clientQueryService.ListProperties(parentId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            var viewModel = new ClientPropertyListModel
            {
                Data = ObjectMapper.Map<List<ClientPropertyDto>, List<PropertyViewModel>>(result.Value),
                Draw = model.Draw
            };


            return Json(viewModel);
        }

        public IActionResult CreatePropertyModal(int parentId)
        {
            return PartialView("_Create.Property", new PropertyViewModel { ParentId = parentId });
        }

        public async Task<IActionResult> EditPropertyModal(int parentId, int propertyId)
        {
            var result = await _clientQueryService.GetProperty(parentId, propertyId);

            if (result.IsFailure)
            {
                return HandleFailureResultWithView(result);
            }

            return PartialView("_Edit.Property", new PropertyViewModel
            {
                ParentId = result.Value.ClientId,
                PropertyId = result.Value.Id,
                Key = result.Value.Key,
                Value = result.Value.Value
            });
        }

        [HttpPost]
        public async Task<IActionResult> CreateProperty(PropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var propertyModel = ObjectMapper.Map<PropertyViewModel, PropertyModel>(model);

            var result = await _clientCommandService.AddProperty(model.ParentId, propertyModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Ok();
        }


        [HttpPost]
        public async Task<IActionResult> UpdateProperty(PropertyViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var propertyModel = ObjectMapper.Map<PropertyViewModel, PropertyModel>(model);

            var result = await _clientCommandService.UpdateProperty(model.ParentId, model.PropertyId, propertyModel);

            if (result.IsFailure)
            {
                return HandleFailureResultWithJson(result);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProperty([FromBody]RemovePropertyModel model)
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


        private ClientModel PrepareClientModel(CreateClientModel model)
        {
            var clientModel = new ClientModel()
            {
                ClientId = model.ClientId,
                ClientName = model.ClientName,
                AllowedGrantTypes = new List<string>()
            };

            switch (model.Type)
            {
                case ClientType.Web:
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

        private async Task<List<SelectListItem>> PrepareClientScopes(EditClientModel model)
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
