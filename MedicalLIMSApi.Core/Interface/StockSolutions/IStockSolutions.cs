using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Entities.StockSolutions;

namespace MedicalLIMSApi.Core.Interface.StockSolutions
{
    public interface IStockSolutions
    {
        GetActionAndRptData StockManageStockSolutionsRequest(StockManageStockSolutionsRequest obj,TransResults tr);

        GetSTOCKStockSolutionsDetails GetSTOCKStockSolutionsDetailsByID(int stockSolutionID,int userRoleID);

        RecordActionDetails StockManageStockSolutionsPreparation(STOCKManageStockSolutionsPreparation obj,TransResults tr);

        RecordActionDetails StockManageStockSolutionsOutput(STOCKManageStockSolutionsOutput obj, TransResults tr);

        SearchResults<GetSTOCKSearchStockSolutions> StockSearchStockSolutions(STOCKSearchStockSolutions obj , short plantID);
    }
}
