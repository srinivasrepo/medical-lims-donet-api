using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Invalidations;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Entities.VolumetricSolution;
using MedicalLIMSApi.Core.Interface.VolumetricSolution;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.VolumetricSolution
{
    public class VolumetricSolutionRepository : IVolumetricSolution
    {
        TrainingContext context = new TrainingContext();
        DBHelper ctx = new DBHelper();

        public VolumetricSolIndexData GetVolumetricSolIndex(VolumetricSolIndexFilter obj, TransResults trn)
        {
            var lst = new VolumetricSolIndexData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspVOLManageSoultionIndex");
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            if (obj.IndexID > default(short))
                ctx.AddInParameter<short>(cmd, "@IndexID", obj.IndexID);
            if (obj.MaterialID > default(int))
                ctx.AddInParameter<int>(cmd, "@MaterialID", obj.MaterialID);
            if (obj.PSMaterialID > default(int))
                ctx.AddInParameter<int>(cmd, "@PSMaterialID", obj.PSMaterialID);
            if (obj.MolecularWeight > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@MolecularWeight", obj.MolecularWeight);
            if (!string.IsNullOrEmpty(obj.FormulaType))
                ctx.AddInParameter<string>(cmd, "@FormulaType", obj.FormulaType);
            if (!string.IsNullOrEmpty(obj.Comments))
                ctx.AddInParameter<string>(cmd, "@Comments", obj.Comments);
            if(!string.IsNullOrEmpty(obj.Status))
                ctx.AddInParameter<string>(cmd,"@Status",obj.Status);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddOutParameter(cmd, "@ResultFlag", System.Data.DbType.String, 25);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<GetVolumetricSolIndex>(reader);

                lst.List = new GetVolumetricSolIndexList();
                foreach (var rr in rrResult)
                    lst.List.Add(rr);

                reader.NextResult();

                if (reader.HasRows)
                {
                    var rrCount = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);
                    foreach (var rr in rrCount)
                        lst.TotalRecords = rr;
                }
            }
            lst.ResultFlag = ctx.GetOutputParameterValue(cmd, "@ResultFlag");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return lst;
        }

        public GetVolumetricSolIndex GetVolumetricSolIndexByID(short indexID)
        {
            var lst = new GetVolumetricSolIndex();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspVOLGetIndexDetailsByID");
            ctx.AddInParameter<short>(cmd, "@IndexID", indexID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<GetVolumetricSolIndex>(reader);
                foreach (var rr in rrResult)
                    lst = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetActionAndRptData ManageVolumetricSol(AddSolution obj, TransResults trn)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspVOLManageVolSolution");

            if (obj.SolutionID > default(int))
                ctx.AddInParameter<int>(cmd, "@SolutionID", obj.SolutionID);

            ctx.AddInParameter<int>(cmd, "@MatID", obj.MaterialID);
            ctx.AddInParameter<decimal>(cmd, "@PreparationVolume", obj.PreparationVolume);
            if (!string.IsNullOrEmpty(obj.BrefDesc))
                ctx.AddInParameter<string>(cmd, "@BrefDesc", obj.BrefDesc);

            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            if (obj.FormulaID > default(int))
                ctx.AddInParameter<int>(cmd, "@FormulaID", obj.FormulaID);
            if (obj.ValidityPeriodID > default(int))
                ctx.AddInParameter<int>(cmd, "@ValidityPeriodID", obj.ValidityPeriodID);
            if (obj.RestandardizationPeriodID > default(int))
                ctx.AddInParameter<int>(cmd, "@RestandardizationPeriodID", obj.RestandardizationPeriodID);
            if (!string.IsNullOrEmpty(obj.PsDryingTem))
                ctx.AddInParameter<string>(cmd, "@PsDryingTemp", obj.PsDryingTem);
            if (!string.IsNullOrEmpty(obj.DryingDuration))
                ctx.AddInParameter<string>(cmd, "@DryingDuration", obj.DryingDuration);
            if (!string.IsNullOrEmpty(obj.CoolingDuration))
                ctx.AddInParameter<string>(cmd, "@CoolingDuration", obj.CoolingDuration);

            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);



            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrResult)
                    retObj.Act = rr;

                reader.NextResult();
                retObj.RptList = new UploadReportList();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<UploadReports>(reader);
                foreach (var rr in rrLst)
                    retObj.RptList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return retObj;
        }

        public GetVolumetricSol GetVolumetricSolutionByID(int solutionID, int userRoleID)
        {
            var lst = new GetVolumetricSol();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspVOLGetVolumetricSolutionByID");
            ctx.AddInParameter<int>(cmd, "@SolutionID", solutionID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", userRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<GetVolumetricSol>(reader);

                foreach (var rr in rrResult)
                    lst = rr;

                reader.NextResult();

                var rrResultSolList = ((IObjectContextAdapter)context).ObjectContext.Translate<Solvents>(reader);
                lst.List = new SolventsList();

                foreach (var rr in rrResultSolList)
                    lst.List.Add(rr);

                reader.NextResult();

                var rrResultAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                lst.Act = new RecordActionDetails();

                foreach (var rr in rrResultAct)
                    lst.Act = rr;

                reader.NextResult();

                var rrResultStand = ((IObjectContextAdapter)context).ObjectContext.Translate<GetStandardization>(reader);
                lst.StandList = new GetStandardizationList();

                foreach (var rr in rrResultStand)
                    lst.StandList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetStandardizationInfo GetVolumetricStandardByID(int standardizationID, TransResults trn)
        {
            var lst = new GetStandardizationInfo();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspVOLGetStandardization");
            ctx.AddInParameter<int>(cmd, "@StandardizationID", standardizationID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<GetStandardizationInfo>(reader);

                foreach (var rr in rrResult)
                    lst = rr;

                reader.NextResult();

                var rrResultSolList = ((IObjectContextAdapter)context).ObjectContext.Translate<Solvents>(reader);
                lst.List = new SolventsList();

                foreach (var rr in rrResultSolList)
                    lst.List.Add(rr);

                reader.NextResult();

                var rrResultStand = ((IObjectContextAdapter)context).ObjectContext.Translate<StandardizationDetails>(reader);
                lst.StdList = new StandardizationDetailsList();

                foreach (var rr in rrResultStand)
                    lst.StdList.Add(rr);


                reader.NextResult();

                var rrResultAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                lst.Act = new RecordActionDetails();

                foreach (var rr in rrResultAct)
                    lst.Act = rr;


                reader.NextResult();

                var rrFormulas = ((IObjectContextAdapter)context).ObjectContext.Translate<VolumetricSolFormulasBO>(reader);
                lst.FormulaList = new VolumetricSolFormulasBOList();

                foreach (var rr in rrFormulas)
                    lst.FormulaList.Add(rr);

                reader.NextResult();

                var rrReview = ((IObjectContextAdapter)context).ObjectContext.Translate<ReviewInvalidationsBO>(reader);
                lst.InvReviewDetails = new ReviewInvalidationsBO();

                foreach (var rr in rrReview)
                    lst.InvReviewDetails = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public RecordActionDetails ManageVolumetricSolStandard(ManageVolStdDetails obj, TransResults trn)
        {

            var rrAct = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspVOLManageVolumeStandardization");

            ctx.AddInParameter<int>(cmd, "@StandardizationID", obj.StandardID);
            if (obj.FinalVol > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@FinalVol", obj.FinalVol);
            if (!string.IsNullOrEmpty(obj.PSDrying))
                ctx.AddInParameter<string>(cmd, "@PSDrying", obj.PSDrying);
            if (!string.IsNullOrEmpty(obj.StdXMLstring))
                ctx.AddInParameter<string>(cmd, "@VolStdXML", obj.StdXMLstring);
            if (obj.Avg > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@Avg", obj.Avg);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            if (!string.IsNullOrEmpty(obj.Remarks))
                ctx.AddInParameter<string>(cmd, "@Remarks", obj.Remarks);
            ctx.AddInParameter<string>(cmd, "@StdProcedure", obj.StdProcedure);
            if (!string.IsNullOrEmpty(obj.CoolingDuration))
                ctx.AddInParameter<string>(cmd, "@CoolingDuration", obj.CoolingDuration);
            if (!string.IsNullOrEmpty(obj.DryingDuration))
                ctx.AddInParameter<string>(cmd, "@DryingDuration", obj.DryingDuration);
            if (!string.IsNullOrEmpty(obj.previousMolarity))
                ctx.AddInParameter<string>(cmd, "@PreviousMolarity", obj.previousMolarity);
            if (!string.IsNullOrEmpty(obj.BlankValue))
                ctx.AddInParameter<string>(cmd, "@BlankValue", obj.BlankValue);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrResult)
                    rrAct = rr;

                reader.NextResult();

                if (reader.HasRows)
                {
                    var rrResultAct = ((IObjectContextAdapter)context).ObjectContext.Translate<string>(reader);

                    foreach (var rr in rrResultAct)
                        rrAct.ResultFlag = rr;
                }

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return rrAct;
        }

        public SearchResults<SearchVolumetricSOl> SearchVolumetricSol(SearchVolumetricSolFilter obj, TransResults tran)
        {
            var lst = new SearchResults<SearchVolumetricSOl>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspVOLSearchVolumetricSol");
            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);
            ctx.AddInParameter<short>(cmd, "@PlantID", tran.PlantID);

            if (!string.IsNullOrEmpty(obj.FormulaType))
                ctx.AddInParameter<string>(cmd, "@FormulaType", obj.FormulaType);

            if (obj.MaterialID > default(int))
                ctx.AddInParameter<int>(cmd, "@MaterialID", obj.MaterialID);

            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.BatchNumberID > default(int))
                ctx.AddInParameter<int>(cmd, "@BatchNumberID", obj.BatchNumberID);
            if (obj.ValidityFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@ValidityFrom", obj.ValidityFrom);
            if (obj.ValidityTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@ValidityTo", obj.ValidityTo);
            if (obj.SolutionIDFrom > default(int))
                ctx.AddInParameter<int>(cmd, "@SolutionIDFrom", obj.SolutionIDFrom);
            if (obj.SolutionIDTo > default(int))
                ctx.AddInParameter<int>(cmd, "@SolutionIDTo", obj.SolutionIDTo);
            if (obj.InitiatedOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@InitiatedOn", obj.InitiatedOn);
            if (obj.InitiatedBy > default(int))
                ctx.AddInParameter<int>(cmd, "@InitiatedBy", obj.InitiatedBy);
            if (obj.SolutionID > default(int))
                ctx.AddInParameter<int>(cmd, "@SolutionID", obj.SolutionID);
            ctx.AddInParameter<int>(cmd, "@UserID", tran.UserID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);

                foreach (var rr in rrResult)
                    lst.TotalNumberOfRows = rr;

                reader.NextResult();

                var rrResultSolList = ((IObjectContextAdapter)context).ObjectContext.Translate<SearchVolumetricSOl>(reader);

                var list = new List<SearchVolumetricSOl>();

                foreach (var rr in rrResultSolList)
                    list.Add(rr);

                lst.SearchList = list;


                cmd.Connection.Close();
            }

            ctx.CloseConnection(context);
            return lst;
        }

        public RecordActionDetails ReStandardization(ReStandardization obj, TransResults trn)
        {
            var lst = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspVOLStartSDorRSD");

            ctx.AddInParameter<int>(cmd, "@SolutionID", obj.SolutionID);
            ctx.AddInParameter<string>(cmd, "@StdType", obj.StdType);
            ctx.AddOutParameter(cmd, "@ResultFlag", System.Data.DbType.String, 25);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<bool>(cmd, "@AddAudit", true);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrResult)
                    lst = rr;

            }
            cmd.Connection.Close();
            lst.ReturnFlag = ctx.GetOutputParameterValue(cmd, "@ResultFlag");
            ctx.CloseConnection(context);
            return lst;
        }

        public string GetRSDValue(string xml)
        {
            string rsd = string.Empty; ;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspVOLGetRSDValue");

            ctx.AddInParameter<string>(cmd, "@ValueXML", xml);
            ctx.AddOutParameter(cmd, "@RsdString", System.Data.DbType.String, 10);

            cmd.ExecuteNonQuery();
            cmd.Connection.Close();
            rsd = ctx.GetOutputParameterValue(cmd, "@RsdString");
            ctx.CloseConnection(context);
            return rsd;
        }

        public TransResultApprovals InvalidateRequest(ReStandardization obj, TransResults trn)
        {
            var lst = new TransResultApprovals();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "analysis.uspINVALInsertInvalidateRequest");

            ctx.AddInParameter<int>(cmd, "@EntityActID", obj.SolutionID);
            ctx.AddInParameter<string>(cmd, "@EntitySourceCode", obj.StdType);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@DeptCode", trn.DeptCode);

            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResult = ((IObjectContextAdapter)context).ObjectContext.Translate<TransResultApprovals>(reader);
                foreach (var rr in rrResult)
                    lst = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public ProcedureUpdate ManageStandProcedures(ProcedureUpdate obj, TransResults trn)
        {
            ProcedureUpdate data = new ProcedureUpdate();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspVOLManageIndexPrecedure");
            ctx.AddInParameter<short>(cmd, "@IndexID", obj.IndexID);
            if (!string.IsNullOrEmpty(obj.PreparationProcedure))
                ctx.AddInParameter<string>(cmd, "@PreparationProcedure", obj.PreparationProcedure);
            if (!string.IsNullOrEmpty(obj.StandardizationProcedure))
                ctx.AddInParameter<string>(cmd, "@StandardizationProcedure", obj.StandardizationProcedure);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ProcedureUpdate>(reader);
                foreach (var rr in rrRes)
                    data = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return data;
        }

        public List<GetSolutionFormulae> getSolutionFormulae(short indexID)
        {
            List<GetSolutionFormulae> obj = new List<GetSolutionFormulae>();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspGetSolutionFormulae");
            ctx.AddInParameter<short>(cmd, "@IndexID", indexID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSolutionFormulae>(reader);
                foreach (var rr in rrRes)
                    obj.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return obj;
        }

        public List<GetSolutionFormulasByIndexID> getSolutionFormulasByIndexID(int indexID)
        {
            List<GetSolutionFormulasByIndexID> obj = new List<GetSolutionFormulasByIndexID>();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspGetSolutionFormulasByIndexID");
            ctx.AddInParameter<int>(cmd, "@IndexID", indexID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetSolutionFormulasByIndexID>(reader);
                foreach (var rr in rrRes)
                    obj.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return obj;
        }

        public string manageSolutionFormula(ManageSolutionFormula obj, TransResults tr)
        {
            var retCode = string.Empty;
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspManageSolutionFormula");
            if (obj.IndexID > default(short))
                ctx.AddInParameter<short>(cmd, "@IndexID", obj.IndexID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            if (obj.formulaID > default(int))
                ctx.AddInParameter<int>(cmd, "@FormulaID", obj.formulaID);
            ctx.AddInParameter<decimal>(cmd, "@StrengthRangeFrom", obj.StrengthRangeFrom);
            ctx.AddInParameter<decimal>(cmd, "@StrengthRangeTo", obj.StrengthRangeTo);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            if (obj.FormulaIndexID > default(int))
                ctx.AddInParameter<int>(cmd, "@FormulaIndexID", obj.FormulaIndexID);
            ctx.AddOutParameter(cmd, "@ReturnFlag", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            retCode = ctx.GetOutputParameterValue(cmd, "@ReturnFlag");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retCode;
        }

        public GetVOLViewSolIndexDetailsByIDResp GetVOLViewSolIndexDetailsByID(int IndexID)
        {
            GetVOLViewSolIndexDetailsByIDResp obj = new GetVOLViewSolIndexDetailsByIDResp();
            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspVOLViewSolIndexDetailsByID");
            ctx.AddInParameter<int>(cmd, "@IndexID", IndexID);
            using (var reader = cmd.ExecuteReader())
            {
                obj.VolSolIndex = new VolSolutionIndex();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<VolSolutionIndex>(reader);
                foreach (var rr in rrRes)
                    obj.VolSolIndex = rr;

                reader.NextResult();
                obj.SolMgmtFormulaeList = new List<SolMgmtFormulae>();
                var rrResp = ((IObjectContextAdapter)context).ObjectContext.Translate<SolMgmtFormulae>(reader);
                foreach (var rr in rrResp)
                    obj.SolMgmtFormulaeList.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);

            return obj;
        }
    }
}
