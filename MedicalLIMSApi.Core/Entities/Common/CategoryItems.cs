using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Entities.Common
{

    public class ActiveStatus
    {
        public int STATUS_ID { get; set; }

        public string STATUS { get; set; }
    }

    public class CatItemsMaster
    {
        public int CatItemID { get; set; }

        public string CatItem { get; set; }

        public string CatItemCode { get; set; }

        public short CategoryID { get; set; }

        public string Status { get; set; }

        public string Category { get; set; }
    }

    public class CatItemsMasterList : List<CatItemsMaster> { }


    public class CategoryMaster
    {
        public short CategoryID { get; set; }

        public string CategoryCode { get; set; }

        public string Category { get; set; }

        public bool IsSysMaster { get; set; }
    }

    public class CategoryMasterList : List<CategoryMaster> { }

    public class searchCat
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public int? CategoryID { get; set; }

        public string CatItem { get; set; }

        public string CatItemCode { get; set; }

        public string EntityType { get; set; }
    }

    public class ValidityEntities
    {
        public int EntityID { get; set; }

        public string EntityName { get; set; }

        public string EntityCode { get; set; }
    }

    public class ValidityEntityList : List<ValidityEntities> { }

    public class AddValidityPeriods
    {
        public string CategoryCode { get; set; }

        public int CatItemID { get; set; }

        public int EntityID { get; set; }

        public string CatItem { get; set; }

        public int Value { get; set; }
    }

    public class GetProductStageDetails
    {
        public int? StageID { get; set; }

        public string Stage { get; set; }

        public string Product { get; set; }

        public int CategoryID { get; set; }

        public int MatID { get; set; }

        public string MatName { get; set; }

        public string MatCode { get; set; }
    }
}
