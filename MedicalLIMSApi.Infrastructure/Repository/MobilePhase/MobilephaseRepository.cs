using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Interface.MobilePhase;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MedicalLIMSApi.Infrastructure.Repository.MobilePhase
{
    public class MobilephaseRepository : IMobilePhase
    {
        private TrainingContext context = new TrainingContext();
        private DBHelper ctx = new DBHelper();

        public GetActionAndRptData ManageMobilePhase(MobilePhaseBO obj, TransResults tr)
        {
            GetActionAndRptData retObj = new GetActionAndRptData();
            retObj.Act = new RecordActionDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspMPManageMobilePhasePreparation");
            if (obj.MobilePhaseID > default(int))
                ctx.AddInParameter<int>(cmd, "@MobilePhaseID", obj.MobilePhaseID);
            if (obj.MaterialID > default(int))
                ctx.AddInParameter<int>(cmd, "@MaterialID", obj.MaterialID);
            if (obj.SpecificationID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecificationID", obj.SpecificationID);
            ctx.AddInParameter<string>(cmd, "@Purpose", obj.Purpose);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            if (obj.StageID > default(int))
                ctx.AddInParameter<int>(cmd, "@StageID", obj.StageID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            if (!string.IsNullOrEmpty(obj.XMLstring))
                ctx.AddInParameter(cmd, "@UploadedIDXMl", obj.XMLstring);
            if (obj.ParameterType > default(int))
                ctx.AddInParameter<int>(cmd, "@ParameterType", obj.ParameterType);
            if (obj.SpecTest > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecTest", obj.SpecTest);
            if (!string.IsNullOrEmpty(obj.Manual))
                ctx.AddInParameter<string>(cmd, "@Manual", obj.Manual);
            if (obj.MaintenanceReportID > default(int))
                ctx.AddInParameter<int>(cmd, "@MaintenanceReportID", obj.MaintenanceReportID);
            if (!string.IsNullOrEmpty(obj.CalibrationReference))
                ctx.AddInParameter<string>(cmd, "@CalibrationReference", obj.CalibrationReference);
            ctx.AddInParameter<string>(cmd, "@PreparationType", obj.PreparationType);


            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    retObj.Act = rr;
                reader.NextResult();
                if (reader.HasRows)
                {
                    var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<UploadReports>(reader);
                    retObj.RptList = new UploadReportList();
                    foreach (var rr in rrLst)
                        retObj.RptList.Add(rr);
                }
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return retObj;
        }

        public MobilePhaseData GetMobilePhaseData(int mobilePhaseID, TransResults trn)
        {
            MobilePhaseData data = new MobilePhaseData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspMPGetMobilePhasePreparationData");
            ctx.AddInParameter<int>(cmd, "@MobilePhaseID", mobilePhaseID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetMobilePhasePreparation>(reader);
                data.MobilePhase = new GetMobilePhasePreparation();
                foreach (var rr in rrRes)
                    data.MobilePhase = rr;

                reader.NextResult();

                var rrSol = ((IObjectContextAdapter)context).ObjectContext.Translate<Solvents>(reader);
                data.Solvents = new SolventsList();
                foreach (var rr in rrSol)
                    data.Solvents.Add(rr);

                reader.NextResult();

                var rrTran = ((IObjectContextAdapter)context).ObjectContext.Translate<TransResultApprovals>(reader);
                data.AppTran = new TransResultApprovals();
                foreach (var rr in rrTran)
                    data.AppTran = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return data;
        }

        public UOMList GetConvertableUOMByMatID(int? materialID, int sioID)
        {
            UOMList lst = new UOMList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "root.uspGetConvertUOMsByMatrialID");
            if (materialID > default(int))
                ctx.AddInParameter<int?>(cmd, "@MaterialID", materialID);
            if (sioID > default(int))
                ctx.AddInParameter<int?>(cmd, "@SioID", sioID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<UOM>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public Preparations ManageMobilePhaseSolventsPreparation(SolventPreparation obj, TransResults tr)
        {
            Preparations lst = new Preparations();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspMPManageMobilePhaseSolventsPreparation");
            if (obj.PreparationID > default(int))
                ctx.AddInParameter<int>(cmd, "@PreparationID", obj.PreparationID);
            ctx.AddInParameter<int>(cmd, "@EntActID", obj.EntityActID);
            //ctx.AddInParameter<int>(cmd, "@BatchID", obj.BatchID);
            //ctx.AddInParameter<decimal>(cmd, "@QuantityPreparation", obj.QuantityPreparation);
            //ctx.AddInParameter<int>(cmd, "@UomID", obj.UomID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (obj.PackInvID > default(int))
                ctx.AddInParameter<int>(cmd, "@PackInvID", obj.PackInvID);
            if (obj.RefInvID > default(int))
                ctx.AddInParameter<int>(cmd, "@RefPackID", obj.RefInvID);
            ctx.AddInParameter<string>(cmd, "@RequestFrom", obj.RequestFrom);

            if (!string.IsNullOrEmpty(obj.SourceType))
                ctx.AddInParameter<string>(cmd, "@SourceType", obj.SourceType);

            ctx.AddInParameter<bool>(cmd, "@IsPrimaryPrepBatch", obj.IsPrimaryPreparationBatch);

            using (var reader = cmd.ExecuteReader())
            {
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                lst.Act = new RecordActionDetails();
                foreach (var rr in rrAct)
                    lst.Act = rr;

                reader.NextResult();

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<Solvents>(reader);
                lst.List = new SolventsList();
                foreach (var rr in rrRes)
                    lst.List.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public TransResultApprovals ManageMobilePhasePrepComments(MobilePhasePrepComments obj, TransResults trn)
        {
            var lst = new TransResultApprovals();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspMPManagePreparationComments");

            ctx.AddInParameter<int>(cmd, "@MobilePhaseID", obj.EntActID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            //ctx.AddInParameter<decimal>(cmd, "@solPH", obj.solPH);


            //if (!string.IsNullOrEmpty(obj.BufferPrep))
            //    ctx.AddInParameter<string>(cmd, "@BufferPrep", obj.BufferPrep);
            //if (!string.IsNullOrEmpty(obj.PhaseA))
            //    ctx.AddInParameter<string>(cmd, "@MobilePhase_A", obj.PhaseA);
            //if (!string.IsNullOrEmpty(obj.PhaseB))
            //    ctx.AddInParameter<string>(cmd, "@MobilePhase_B", obj.PhaseB);
            //if (!string.IsNullOrEmpty(obj.DiluentPrep))
            //    ctx.AddInParameter<string>(cmd, "@DiluentPhase", obj.DiluentPrep);
            if (!string.IsNullOrEmpty(obj.OtherInfo))
                ctx.AddInParameter<string>(cmd, "@OtherInfo", obj.OtherInfo);
            //else
            //    ctx.AddInParameter<string>(cmd, "@OtherInfo", obj.JsonSerial);

            ctx.AddInParameter<string>(cmd, "@PreparationXML", obj.PreparationXML);


            using (var reader = cmd.ExecuteReader())
            {
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<TransResultApprovals>(reader);
                foreach (var rr in rrAct)
                    lst = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public SingleUsrBasicList GetValidityPeriods(string entityCode)
        {
            var lst = new SingleUsrBasicList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspGetValidityPeriods");
            ctx.AddInParameter<string>(cmd, "@EntityCode", entityCode);
            using (var reader = cmd.ExecuteReader())
            {
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<SingleUsrBasic>(reader);
                foreach (var rr in rrAct)
                    lst.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public ManageMobilePreparationOutput ManagePhaseOutput(MobilePhaseOutput obj, TransResults trn)
        {
            var lst = new ManageMobilePreparationOutput();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspMPManageMobilePhaseOutput");

            ctx.AddInParameter<int>(cmd, "@MobilePhaseID", obj.EntActID);
            ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);

            if (obj.List.Count > 0)
                ctx.AddInParameter<string>(cmd, "@PreparationXML", obj.XMLStringOutPut);

            if (!string.IsNullOrEmpty(obj.OtherInfo))
                ctx.AddInParameter<string>(cmd, "@OtherInfo", obj.OtherInfo);

            ctx.AddInParameter<decimal>(cmd, "@FinaVolume", obj.FinalVol);

            using (var reader = cmd.ExecuteReader())
            {
                lst.Trn = new TransResultApprovals();
                var rrAct = ((IObjectContextAdapter)context).ObjectContext.Translate<TransResultApprovals>(reader);
                foreach (var rr in rrAct)
                    lst.Trn = rr;

                reader.NextResult();

                var rrOut = ((IObjectContextAdapter)context).ObjectContext.Translate<ManageMobilePreparationOutput>(reader);
                foreach (var rr in rrOut)
                {
                    lst.FinalVolume = rr.FinalVolume;
                    lst.UseBeforeDateTime = rr.UseBeforeDateTime;
                }

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }
        public SearchMobilePhaseData GetSearchMobilePhaseData(SearchMPBO obj)
        {
            SearchMobilePhaseData data = new SearchMobilePhaseData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspMPSerchMobilePhaseData");
            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<byte>(cmd, "@PageSize", obj.PageSize);
            if (obj.Purpose > default(int))
                ctx.AddInParameter<int>(cmd, "@Purpose", obj.Purpose);
            if (obj.ProductID > default(int))
                ctx.AddInParameter<int>(cmd, "@ProductID", obj.ProductID);
            if (obj.StageID > default(int))
                ctx.AddInParameter<int>(cmd, "@StageID", obj.StageID);
            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);
            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.BatchNumber > default(int))
                ctx.AddInParameter<int>(cmd, "@BatchNumber", obj.BatchNumber);
            if (obj.SpecificationID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecificationID", obj.SpecificationID);
            if (obj.SpecTestID > default(int))
                ctx.AddInParameter<int>(cmd, "@SpecTestID", obj.SpecTestID);
            if (obj.MobilePhaseIDFrom > default(int))
                ctx.AddInParameter<int>(cmd, "@MobilePhaseIDFrom", obj.MobilePhaseIDFrom);
            if (obj.MobilePhaseIDTo > default(int))
                ctx.AddInParameter<int>(cmd, "@MobilePhaseIDTo", obj.MobilePhaseIDTo);
            if (obj.ValidFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@ValidFrom", obj.ValidFrom);
            if (obj.ValidTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@ValidTo", obj.ValidTo);
            if (obj.PreparationType > default(int))
                ctx.AddInParameter<int>(cmd, "@PreparationType", obj.PreparationType);
            if (obj.InitiatedBy > default(int))
                ctx.AddInParameter<int>(cmd, "@InitiatedBy", obj.InitiatedBy);
            if (obj.InitiatedOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@InitiatedOn", obj.InitiatedOn);
            if (obj.MobilePhaseID > default(int))
                ctx.AddInParameter<int>(cmd, "@MobilePhaseID",obj.MobilePhaseID);
            ctx.AddInParameter<short>(cmd, "@PlantID", obj.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrTotal = ((IObjectContextAdapter)context).ObjectContext.Translate<SearchMobilePhaseData>(reader);
                foreach (var rr in rrTotal)
                    data = rr;

                reader.NextResult();

                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<MPData>(reader);
                data.List = new MPList();
                foreach (var rr in rrRes)
                    data.List.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return data;
        }

        public viewMobilePhase ViewMobilePhaseData(int phaseID)
        {
            viewMobilePhase obj = new viewMobilePhase();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspMPGetViewMobilePhaseData");
            ctx.AddInParameter<int>(cmd, "@MobilePhaseID", phaseID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetMobilePhasePreparation>(reader);
                obj.MobilePhase = new GetMobilePhasePreparation();
                foreach (var rr in rrRes)
                    obj.MobilePhase = rr;

                reader.NextResult();

                var rrSol = ((IObjectContextAdapter)context).ObjectContext.Translate<Solvents>(reader);
                obj.Solvents = new SolventsList();
                foreach (var rr in rrSol)
                    obj.Solvents.Add(rr);

                reader.NextResult();

                var rrPrep = ((IObjectContextAdapter)context).ObjectContext.Translate<GetPreparationDetails>(reader);
                obj.Preparation = new GetPreparationDetailsList();
                foreach (var rr in rrPrep)
                    obj.Preparation.Add(rr);

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }

        public GetPreparationDetailsList GetPreparationDetails(int phaseID)
        {
            var lst = new GetPreparationDetailsList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspMPGetPreparationDetails");
            ctx.AddInParameter<int>(cmd, "@MobilePhaseID", phaseID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetPreparationDetails>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public ParameterTypeList GetCalibrationParameters()
        {
            ParameterTypeList lst = new ParameterTypeList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspGetCalibrationParameters");

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ParameterType>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetMasterData ManagePreparationMasterData(MasterData obj, TransResults tr)
        {
            GetMasterData data = new GetMasterData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspManageMobilePhaseMasterData");
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            ctx.AddInParameter<string>(cmd, "@PreparationType", obj.PreparationType);
            ctx.AddInParameter<string>(cmd, "@TypeCode", obj.TypeCode);
            if (obj.MaterialID > default(int))
                ctx.AddInParameter<int>(cmd, "@MaterialID", obj.MaterialID);
            if (obj.TestID > default(int))
                ctx.AddInParameter<int>(cmd, "@TestID", obj.TestID);
            if (!string.IsNullOrEmpty(obj.PreparationXml))
                ctx.AddInParameter<string>(cmd, "@PreparationXml", obj.PreparationXml);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            ctx.AddInParameter<int>(cmd, "@MobilePhaseID", obj.MobilePhaseID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetMasterData>(reader);
                foreach (var rr in rrRes)
                    data = rr;

                reader.NextResult();

                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<PreparationData>(reader);
                data.Lst = new PreparationDataList();
                foreach (var r in rrLst)
                    data.Lst.Add(r);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return data;
        }

        public string DiscardPreparationBatch(DiscardPreparationBatch obj, TransResults tr)
        {
            string retMsg = string.Empty;

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspDiscardPreparationBatch");
            ctx.AddInParameter<int>(cmd, "@PreparationID", obj.PreparationID);
            ctx.AddInParameter<bool>(cmd, "@CanInvalidate", obj.CanInvalidate);

            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);

            ctx.AddOutParameter(cmd, "@Retcode", System.Data.DbType.String, 25);
            cmd.ExecuteNonQuery();
            retMsg = ctx.GetOutputParameterValue(cmd, "@Retcode");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return retMsg;
        }

        public GetProductStageDetails GetMaterialDetailsBySpecID(int specID)
        {
            GetProductStageDetails obj = new GetProductStageDetails();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.GetMaterialDetailsBySpecID");
            ctx.AddInParameter<int>(cmd, "@SpecID", specID);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetProductStageDetails>(reader);
                foreach (var rr in rrRes)
                    obj = rr;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return obj;
        }
    }
}
