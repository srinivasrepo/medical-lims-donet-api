using MedicalLIMSApi.Core.CommonMethods;
using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Entities.SampleAnalysis;
using MedicalLIMSApi.Core.Entities.VolumetricSolution;
using MedicalLIMSApi.Core.Interface.VolumetricSolution;
using MedicalLIMSApi.Web.App_Start;
using MedicalLIMSApi.Web.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Web.Http;

namespace MedicalLIMSApi.Web.Controllers.VolumetricSolution
{

    [LIMSAuthorization]

    public class VolumetricSolutionController : ApiController
    {

        IVolumetricSolution db;

        public VolumetricSolutionController(IVolumetricSolution db)
        {
            this.db = db;
        }


        [HttpPost]
        [Route("GetVolumetricSolIndex")]
        public VolumetricSolIndexData GetVolumetricSolIndex(VolumetricSolIndexFilter obj)
        {
            return db.GetVolumetricSolIndex(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }


        [HttpGet]
        [Route("GetVolumetricSolIndexByID")]
        public GetVolumetricSolIndex GetVolumetricSolIndexByID(string encIndexID)
        {
            return db.GetVolumetricSolIndexByID(CommonStaticMethods.Decrypt<short>(encIndexID));
        }


        [HttpPost]
        [Route("ManageVolumetricSol")]
        public RecordActionDetails ManageVolumetricSol(AddSolution obj)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();

            retObj = db.ManageVolumetricSol(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());

            if (retObj.Act.ReturnFlag == "SUCCESS" && retObj.RptList != null && retObj.RptList.Count > 0)
            {
                ReportUploadDMS dmsObj = new ReportUploadDMS();
                dmsObj.EntActID = retObj.Act.TransKey;
                dmsObj.EntityCode = "VOLSOLUTION";
                dmsObj.ReferenceNumber = retObj.Act.ReferenceNumber;
                dmsObj.List = retObj.RptList;
                string retCode = FileUploadUtility.UploadReportInfoToDMS(dmsObj);
                if (retCode != "OK" && retCode != "SUCCESS")
                    retObj.Act.ReturnFlag = retCode;
            }

            return retObj.Act;
        }

        [HttpGet]
        [Route("GetVolumetricSolutionByID")]
        public GetVolumetricSol GetVolumetricSolutionByID(string encSolutionID)
        {
            return db.GetVolumetricSolutionByID(CommonStaticMethods.Decrypt<int>(encSolutionID), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails().UserRoleID);
        }

        [HttpGet]
        [Route("GetVolumetricStandardByID")]
        public GetStandardizationInfo GetVolumetricStandardByID(string encStandard)
        {
            return db.GetVolumetricStandardByID(CommonStaticMethods.Decrypt<int>(encStandard), MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageVolumetricSolStandard")]
        public RecordActionDetails ManageVolumetricSolStandard(ManageVolStdDetails obj)
        {

            var lst = new ExecuteFormula();
            lst.Formula = obj.FormulaDef;
            lst.Lst = null;
            FormulaDepenDetails input = null;
            string formula = string.Empty;
            string calValue = string.Empty;
            decimal volConsumed = default(decimal);
            decimal BlankValue = default(decimal);
            decimal.TryParse(obj.BlankValue, out BlankValue);
            foreach (var item in obj.StdList)
            {
                formula = obj.FormulaDef;
                lst.Lst = new FormulaDependentList();
                input = new FormulaDepenDetails();
                input.InputCode = "A1";
                input.Value = Convert.ToString(item.PSWeight);
                lst.Lst.Add(input);
                formula = formula.Replace("A1", Convert.ToString(item.PSWeight));
                decimal.TryParse(item.VolConsumed, out volConsumed);

                input = new FormulaDepenDetails();
                input.InputCode = "A2";
                input.Value = Convert.ToString(volConsumed - BlankValue);
                lst.Lst.Add(input);
                formula = formula.Replace("A2", input.Value);

                input = new FormulaDepenDetails();
                input.InputCode = "A3";
                input.Value = Convert.ToString(obj.previousMolarity);
                lst.Lst.Add(input);
                formula = formula.Replace("A3", Convert.ToString(obj.previousMolarity));
                try
                {
                    calValue = new DataTable().Compute(formula, null).ToString();
                    item.Result = Convert.ToString(CommonStaticMethods.TruncateDecimal(calValue));
                }
                catch
                {

                    calValue = CommonStaticMethods.ConvertToFriendlyDecimal(Convert.ToDecimal(CommonStaticMethods.PostApiConnectionData<string>("ExecuteFormula", lst)));
                    item.Result = calValue;
                }
            }

            return db.ManageVolumetricSolStandard(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("SearchVolumetricSol")]
        public SearchResults<SearchVolumetricSOl> SearchVolumetricSol(SearchVolumetricSolFilter obj)
        {
            return db.SearchVolumetricSol(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ReStandardization")]
        public RecordActionDetails ReStandardization(ReStandardization obj)
        {
            return db.ReStandardization(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("GetRSDValue")]

        public string GetRSDValue(VolStdDetailsList list)
        {
            string xml = CommonStaticMethods.Serialize<VolStdDetailsList>(list);
            return db.GetRSDValue(xml);
        }

        [HttpPost]
        [Route("InvalidateRequest")]
        public TransResultApprovals InvalidateRequest(ReStandardization obj)
        {
            return db.InvalidateRequest(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpPost]
        [Route("ManageStandProcedures")]
        public ProcedureUpdate ManageStandProcedures(ProcedureUpdate obj)
        {
            return db.ManageStandProcedures(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetSolutionFormulae")]
        public List<GetSolutionFormulae> getSolutionFormulae(string encIndexID)
        {
            return db.getSolutionFormulae(CommonStaticMethods.Decrypt<short>(encIndexID));
        }

        [HttpPost]
        [Route("ManageSolutionFormula")]
        public string manageSolutionFormula(ManageSolutionFormula obj)
        {
            return db.manageSolutionFormula(obj, MedicalLIMSApi.Web.Utilities.Common.GetUserDetails());
        }

        [HttpGet]
        [Route("GetSolutionFormulasByIndexID")]
        public List<GetSolutionFormulasByIndexID> getSolutionFormulasByIndexID(int indexID)
        {
            return db.getSolutionFormulasByIndexID(indexID);
        }

        [HttpGet]
        [Route("GetVOLViewSolIndexDetailsByID")]
        public GetVOLViewSolIndexDetailsByIDResp GetVOLViewSolIndexDetailsByID(string EncIndexID)
        {
            return db.GetVOLViewSolIndexDetailsByID(CommonStaticMethods.Decrypt<int>(EncIndexID));
        }

    }
}
