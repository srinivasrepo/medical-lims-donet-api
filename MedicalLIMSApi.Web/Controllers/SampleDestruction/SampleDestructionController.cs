using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SampleDestruction;
using MedicalLIMSApi.Core.Interface.SampleDestruction;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.SampleDestruction
{
    public class SampleDestructionController : ApiController
    {
        ISampleDestruction db;
        
        public SampleDestructionController(ISampleDestruction _db)
        {
            this.db = _db;
        } 

        [HttpPost]
        [Route("GetPacksForDestruction")]
        public GetResultPacksForDestructionList GetPacksForDestruction(GetPacksForDestruction obj)
        {
            return db.GetPacksForDestruction(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID);
        }

        [HttpGet]
        [Route("GetDestructionSamples")]
        public GetResultsDestructionSamplesDetails GetResultsDestructionSamples(string encDestructionID)
        {
            return db.GetResultsDestructionSamples(CommonStaticMethods.Decrypt<int>(encDestructionID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SavePacksForDestruction")]
        public GetPacksForDestructionDetails SavePacksForDestruction(SavePacksForDestruction obj)
        {
            return db.SavePacksForDestruction(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageDestructionSamples")]
        public RecordActionDetails ManageDestructionSamples(ManageDestructionSamples obj)
        {
            return db.ManageDestructionSamples(obj,MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SearchSampleDestruction")]
        public SearchResults<GetSampleDestruction> GetSampleDestruction(SearchSampleDestruction obj)
        {
            return db.GetSampleDestruction(obj,MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().PlantID);
        }

        [HttpPost]
        [Route("DiscardSamples")]
        public GetResultDiscardSample DiscardSamples(DiscardSamples obj)
        {
            return db.DiscardSamples(obj,MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetSampleDestructionDetailsForView")]
        public GetSampleDestructionDetailsForView GetSampleDestructionDetailsForView(string encDestructionID)
        {
            return db.GetSampleDestructionDetailsForView(CommonStaticMethods.Decrypt<int>(encDestructionID));
        }

      

    }
}
