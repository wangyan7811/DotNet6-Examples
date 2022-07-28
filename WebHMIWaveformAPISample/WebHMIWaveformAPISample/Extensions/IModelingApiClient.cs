using Refit;

namespace ApiClient.ModelingApiClient;

public interface IModelingApiClient
{
    [Get("/api/v1.0/Equipments/selected-count")]
    Task<string> GetEquipmentSelectedLongCountAsync([Header("Authorization")] string authorization);

    [Get("/api/v1.0/external-pages/{menu-id}")]
    Task<ExternalPage> GetExternalPageByMenuIdAsync([Header("Authorization")] string authorization, [AliasAs("menu-id")] int menuId);

    [Get("/api/v1.0/externalsystem/{id}")]
    Task<ExternalSystem> GetExternalSystemByIdAsync([Header("Authorization")] string authorization, [AliasAs("id")] int id);

    [Get("/api/v1.0/Extractors/all-extractor-and-site")]
    Task<List<ExtractorSiteView>> GetExtractorAndSiteAsync([Header("Authorization")] string authorization);

    [Get("/api/v1.0/trees/energy-navigator")]
    Task<List<AbsTreeNodeView>> GetTreesEnergyNavigatorAsync([Header("Authorization")] string authorization, [Query] TreesEnergyNavigatorQuery query);

    [Get("/api/v1.0/trees/energy-measurements")]
    Task<List<AbsTreeNodeView>> GetTreesEnergyMeasurementsAsync([Header("Authorization")] string authorization, [Query] TreesEnergyMeasurementsQuery query);

    [Get("/api/v1.0/energy-cost")]
    Task<EnergyCostView> GetEnergyCostConfigsBySiteIdAsync([Header("Authorization")] string authorization, [Query] int siteId);

    [Get("/api/v1.0/energy-costs/sites")]
    Task<List<EnergyCostHistoryView>> GetEnergyCostsOfSitesAsync([Header("Authorization")] string authorization, [Query] EnergyCostsSitesQuery query);

    [Get("/api/v1.0/energy-costs/project")]
    Task<List<EnergyCostHistoryView>> GetEnergyCostsOfProjectAsync([Header("Authorization")] string authorization, [Query] EnergyCostsProjectQuery query);

    [Get("/api/v1.0/projects/default")]
    Task<ProjectView> GetProjectOfDefaultAsync([Header("Authorization")] string authorization);

    [Get("/api/v1.0/projects/{id}")]
    Task<ProjectView> GetProjectByIdAsync([Header("Authorization")] string authorization, [AliasAs("id")] int id);

    [Get("/api/v1.0/sites/by-project/{project-id}")]
    Task<SiteView[]> GetSitesByProjectIdAsync([Header("Authorization")] string authorization, [AliasAs("project-id")] int projectId);

    [Get("/api/v1.0/virtual-nodes/{id}")]
    Task<VirtualNodeManageView> GetVirtualNodeByIdAsync([Header("Authorization")] string authorization, [AliasAs("id")] int id);

    [Get("/api/v1.0/virtual-nodes")]
    Task<List<VirtualNodeManageView>> GetVirtualNodesByIdsAsync([Header("Authorization")] string authorization, [AliasAs("id"), Query(CollectionFormat.Multi)] params int[] ids);

    [Get("/api/v1.0/projects/offset")]
    Task<OffsetView> GetProjectOffset([Header("Authorization")] string authorization);
}
