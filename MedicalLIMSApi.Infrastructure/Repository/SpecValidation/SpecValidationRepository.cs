using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SpecValidation;
using MedicalLIMSApi.Core.Interface.SpecValidation;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.SpecValidation
{
    public class SpecValidationRepository : ISpecValidation
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public SpecValidationCycle ManageSpecValidationsDetails(ManageSpecValidation obj, TransResults tr)
        {
            var lst = new SpecValidationCycle();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "spec.uspSPEVALIDManageSpecValidation");
            ctx.AddInParameter<int>(cmd, "@SpecTestCatID", obj.SpecTestCatID);
            ctx.AddInParameter<int>(cmd, "@Mode", obj.Mode);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (obj.StpTemplateID > default(int))
                ctx.AddInParameter<int>(cmd, "@StpTemplateID", obj.StpTemplateID);
            using (var reader = cmd.ExecuteReader())
            {
                lst.Act = new RecordActionDetails();
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrAct)
                    lst.Act = rr;

                reader.NextResult();
                lst.SpecValidtions = new List<GetSpecCycleDetails>();
                var rrSpecValid = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSpecCycleDetails>(reader);
                foreach (var rr in rrSpecValid)
                    lst.SpecValidtions.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }


        public SpecValidationCycle ManageCycleDetails(ManageCycleDetailsBO obj, TransResults tr)
        {
            var lst = new SpecValidationCycle();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "spec.uspSPEVALIDManageCycle");
            ctx.AddInParameter<int>(cmd, "@SpecValidationID", obj.SpecValidationID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            if (obj.SpecValidCycleID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecValidCycleID", obj.SpecValidCycleID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            using (var reader = cmd.ExecuteReader())
            {

                lst.Act = new RecordActionDetails();
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrAct)
                    lst.Act = rr;

                reader.NextResult();
                lst.SpecValidtions = new List<GetSpecCycleDetails>();
                var rrSpecValid = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSpecCycleDetails>(reader);
                foreach (var rr in rrSpecValid)
                    lst.SpecValidtions.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetSpecValidationDetails GetSpecValidationDetails(EditSpecValidation obj)
        {
            var lst = new GetSpecValidationDetails();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "spec.uspSPEVALIDGetSpecValidationDetails");

            ctx.AddInParameter<int>(cmd, "@SpecValidationID", obj.SpecValidationID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", obj.UserRoleID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            using (var reader = cmd.ExecuteReader())
            {
                lst.GetSpecTestDetailsBO = new GetSpecTestDetails();
                var getSpecTest = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSpecTestDetails>(reader);
                foreach (var rr in getSpecTest)
                    lst.GetSpecTestDetailsBO = rr;
                reader.NextResult();

                lst.SpecCycleDetail = new List<GetSpecCycleDetails>();
                var specCycle = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSpecCycleDetails>(reader);
                foreach (var rr in specCycle)
                    lst.SpecCycleDetail.Add(rr);
                reader.NextResult();

                lst.RecordActionResults = new RecordActionDetails();
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrAct)
                    lst.RecordActionResults = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public SearchResults<SearchResultSpecValidations> SearchResultSpecValidations(SearchSpecValidations obj)
        {
            var lst = new SearchResults<SearchResultSpecValidations>();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "spec.uspSPEVALIDSearchSpecValidations");

            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            if (obj.TestID > default(int))
                ctx.AddInParameter<int>(cmd, "@TestID", obj.TestID);
            if (obj.Mode > default(int))
                ctx.AddInParameter<int>(cmd, "@Mode", obj.Mode);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            ctx.AddInParameter<short>(cmd, "@PlantID", obj.PlantID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (obj.SpecID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecID", obj.SpecID);
            if (obj.TemplateID > default(int))
                ctx.AddInParameter<int>(cmd, "@TemplateID", obj.TemplateID);
            if (obj.SpecTypeID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecTypeID", obj.SpecTypeID);
            if (obj.DateFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateFrom", obj.DateFrom);
            if (obj.DateTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@DateTo", obj.DateTo);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.InstrumentTypeID > default(int))
                ctx.AddInParameter<int>(cmd, "@InstrumentTypeID", obj.InstrumentTypeID);
            if (obj.InitiatedBy > default(int))
                ctx.AddInParameter<int>(cmd, "@InitiatedBy", obj.InitiatedBy);
            if (obj.InitiatedOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@InitiatedOn", obj.InitiatedOn);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResCount = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                foreach (var rr in rrResCount)
                    lst.TotalNumberOfRows = rr;
                reader.NextResult();

                List<SearchResultSpecValidations> list = new List<SearchResultSpecValidations>();
                var rrSpecResult = ((IObjectContextAdapter)context).ObjectContext.Translate<SearchResultSpecValidations>(reader);
                foreach (var rr in rrSpecResult)
                    list.Add(rr);
                lst.SearchList = list;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public string ValidateTest(Validate obj)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "spec.uspSPECValidateTest");
            ctx.AddInParameter<int>(cmd, "@TestCatID", obj.TestCatID);
            ctx.AddInParameter<int>(cmd, "@EntActID", obj.EntActID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddOutParameter(cmd, "@RetMsg", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public List<GetSpecificationTestToAssignSTP> GetSpecificationTestToAssignSTP(int specID, int calibID)
        {
            List<GetSpecificationTestToAssignSTP> assignSTPObj = new List<GetSpecificationTestToAssignSTP>();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "spec.uspGetSpecificationTestToAssignSTP");
            if (specID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecID", specID);
            if(calibID > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibID", calibID);
            using (var reader = cmd.ExecuteReader())
            {
                var getAssignSTPObj = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSpecificationTestToAssignSTP>(reader);
                foreach (var rr in getAssignSTPObj)
                    assignSTPObj.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return assignSTPObj;
        }

        public string AssignSTPToTest(AssignSTPToTest obj,TransResults trn)
        {
            var retCode = string.Empty;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "spec.uspAssignSTPToTest");
            if (obj.SpecID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecID", obj.SpecID);
            if (obj.CalibID > default(int))
                ctx.AddInParameter<int>(cmd, "@CalibID", obj.CalibID);
            if(obj.TemplateID > default(int))
                ctx.AddInParameter<int>(cmd, "@TemplateID", obj.TemplateID);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            ctx.AddInParameter<string>(cmd, "@TestXML",obj.TestXML);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddOutParameter(cmd,"@RetCode", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            retCode = ctx.GetOutputParameterValue(cmd, "@RetCode");

            cmd.Connection.Close();
            ctx.CloseConnection(context);

            return retCode;
        }

        public List<TestSTPHistory> TestSTPHistory(int specCatID)
        {
            List<TestSTPHistory> lst = new List<TestSTPHistory>();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "spec.uspGetSpecTestSTPHistory");
            ctx.AddInParameter<int>(cmd, "@SpecCatID", specCatID);
            using (var reader = cmd.ExecuteReader())
            {
                var getAssignSTPObj = ((IObjectContextAdapter)context).ObjectContext.Translate<TestSTPHistory>(reader);
                foreach (var rr in getAssignSTPObj)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }
    }
}
