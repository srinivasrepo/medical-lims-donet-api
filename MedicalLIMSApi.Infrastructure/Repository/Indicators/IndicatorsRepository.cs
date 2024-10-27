using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.Indicators;
using MedicalLIMSApi.Core.Entities.MobilePhase;
using MedicalLIMSApi.Core.Entities.VolumetricSolution;
using MedicalLIMSApi.Core.Interface.Indicators;
using MedicalLIMSApi.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;

namespace MedicalLIMSApi.Infrastructure.Repository.Indicators
{
    public class IndicatorsRepository : IIndicators
    {
        TrainingContext context = new TrainingContext();
        DBHelper ctx = new DBHelper();

        public SearchResults<SearchIndicatorData> SearchIndicators(SearchIndicatorsBO obj)
        {
            var lst = new SearchResults<SearchIndicatorData>();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspINDSearchIndicators");
            ctx.AddInParameter<int>(cmd, "@PageIndex", obj.PageIndex);
            ctx.AddInParameter<int>(cmd, "@PageSize", obj.PageSize);

            if (obj.IndicatorID > default(int))
                ctx.AddInParameter<int>(cmd, "@IndicatorID", obj.IndicatorID);

            if (obj.StatusID > default(int))
                ctx.AddInParameter<int>(cmd, "@StatusID", obj.StatusID);

            if (obj.IndicatorType > default(int))
                ctx.AddInParameter<int>(cmd, "@IndicatorType", obj.IndicatorType);

            if (obj.SolutionID > default(int))
                ctx.AddInParameter<int>(cmd, "@SolutionID", obj.SolutionID);

            if (!string.IsNullOrEmpty(obj.AdvanceSearch))
                ctx.AddInParameter<string>(cmd, "@AdvanceSearch", obj.AdvanceSearch);
            if (obj.IndicatorIDTo > default(int))
                ctx.AddInParameter<int>(cmd, "@IndicatorIDTo", obj.IndicatorIDTo);
            if (obj.BatchNumberID > default(int))
                ctx.AddInParameter<int>(cmd, "@BatchNumberID", obj.BatchNumberID);
            if (obj.ValidityFrom > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@ValidityFrom", obj.ValidityFrom);
            if (obj.ValidityTo > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@ValidityTo", obj.ValidityTo);
            if (obj.InitiatedBy > default(int))
                ctx.AddInParameter<int>(cmd, "@InitiatedBy",obj.InitiatedBy);
            if (obj.InitiatedOn > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@InitiatedOn",obj.InitiatedOn);
            if (obj.IndicatorCodeID > default(int))
                ctx.AddInParameter<int>(cmd, "@IndicatorCodeID",obj.IndicatorCodeID);
            ctx.AddInParameter<short>(cmd, "@PlantID", obj.PlantID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResultInfo = ((IObjectContextAdapter)context).ObjectContext.Translate<int>(reader);

                foreach (var rr in rrResultInfo)
                    lst.TotalNumberOfRows = rr;

                reader.NextResult();

                var rrResultSol = ((IObjectContextAdapter)context).ObjectContext.Translate<SearchIndicatorData>(reader);

                var searchDataList = new List<SearchIndicatorData>();

                foreach (var rr in rrResultSol)
                    searchDataList.Add(rr);

                lst.SearchList = searchDataList;
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetIndicatorsInfo GetIndicatorsInfo(int indicatorID, int userRoleID)
        {
            var lst = new GetIndicatorsInfo();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspINDGetSolutionDetails");
            ctx.AddInParameter<int>(cmd, "@IndicatorID", indicatorID);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", userRoleID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrResultInfo = ((IObjectContextAdapter)context).ObjectContext.Translate<GetIndicatorsInfo>(reader);

                foreach (var rr in rrResultInfo)
                    lst = rr;

                reader.NextResult();

                var rrResultSol = ((IObjectContextAdapter)context).ObjectContext.Translate<Solvents>(reader);

                lst.SolventsList = new SolventsList();

                foreach (var rr in rrResultSol)
                    lst.SolventsList.Add(rr);

                reader.NextResult();

                var rrResultTran = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                lst.Tran = new RecordActionDetails();
                foreach (var rr in rrResultTran)
                    lst.Tran = rr;

                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetActionAndRptData ManageIndicatorsInfo(ManageIndicatorsInfo obj, TransResults trn)
        {
            var lst = new GetActionAndRptData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspINDManageSolutionDetails");

            if (obj.IndicatorID > default(int))
                ctx.AddInParameter<int>(cmd, "@IndicatorID", obj.IndicatorID);

            ctx.AddInParameter<int>(cmd, "@IndicatorTypeID", obj.IndicatorType);
            ctx.AddInParameter<int>(cmd, "@SolutionID", obj.IndicatorSol);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", trn.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", trn.DeptID);
            ctx.AddInParameter<int>(cmd, "@PlantID", trn.PlantID);

            if (!string.IsNullOrEmpty(obj.BriefDescription))
                ctx.AddInParameter<string>(cmd, "@BreafDescription", obj.BriefDescription);
            if (obj.VolumePrepared > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@VolumePrepared", obj.VolumePrepared);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            if (obj.UseBefore > default(DateTime))
                ctx.AddInParameter<DateTime>(cmd, "@UseBefore", obj.UseBefore);
            if (!string.IsNullOrEmpty(obj.InitTime))
                ctx.AddInParameter<string>(cmd, "@InitTime", obj.InitTime);
            if (!string.IsNullOrEmpty(obj.XMLuploadedID))
                ctx.AddInParameter<string>(cmd, "@UploadedIDXMl", obj.XMLuploadedID);
            if (!string.IsNullOrEmpty(obj.OtherInfo))
                ctx.AddInParameter<string>(cmd, "@OtherInfo", obj.OtherInfo);
            if (obj.ValidatePeriodID > default(int))
                ctx.AddInParameter<int>(cmd, "@ValidatePeriodID", obj.ValidatePeriodID);
            if (obj.SolutionPH > default(decimal))
                ctx.AddInParameter<decimal>(cmd, "@SolutionPH",obj.SolutionPH);
            if (!string.IsNullOrEmpty(obj.Weight))
                ctx.AddInParameter<string>(cmd, "@Weight",obj.Weight);
            if (!string.IsNullOrEmpty(obj.DryingTemp))
                ctx.AddInParameter(cmd, "@DryingTemp", obj.DryingTemp);
            if (!string.IsNullOrEmpty(obj.DryingDuration))
                ctx.AddInParameter(cmd, "@DryingDuration", obj.DryingDuration);
            if (!string.IsNullOrEmpty(obj.CoolingDuration))
                ctx.AddInParameter(cmd, "@CoolingDuration", obj.CoolingDuration);

            using (var reader = cmd.ExecuteReader())
            {
                lst.Act = new RecordActionDetails();
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<RecordActionDetails>(reader);
                foreach (var rr in rrRes)
                    lst.Act = rr;

                reader.NextResult();

                lst.RptList = new UploadReportList();
                var rrLst = ((IObjectContextAdapter)context).ObjectContext.Translate<UploadReports>(reader);
                foreach (var rr in rrLst)
                    lst.RptList.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public ViewIndicatorInfo ViewIndicatorInfo(int indicatorID)
        {
            var lst = new ViewIndicatorInfo();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspINDViewIndicatorDetails");
            ctx.AddInParameter<int>(cmd, "@IndicatorID", indicatorID);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<ViewIndicatorInfo>(reader);
                foreach (var rr in rrRes)
                    lst = rr;

                reader.NextResult();

                var rrSol = ((IObjectContextAdapter)context).ObjectContext.Translate<Solvents>(reader);
                lst.SolventList = new SolventsList();

                foreach (var rr in rrSol)
                    lst.SolventList.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public SolventsList GetPreparationDetails(SolventPreparation obj)
        {
            SolventsList lst = new SolventsList();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspGetPreparationDetails");
            ctx.AddInParameter(cmd, "@EntityActID", obj.EntityActID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            if (!string.IsNullOrEmpty(obj.SourceType))
                ctx.AddInParameter<string>(cmd, "@SourceType", obj.SourceType);

            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<Solvents>(reader);
                foreach (var rr in rrRes)
                    lst.Add(rr);
                cmd.Connection.Close();
            }
            ctx.CloseConnection(context);
            return lst;
        }

        public GetINDMasterData ManageIndicatorMasterData(ManageMasterData obj, TransResults tr)
        {
            GetINDMasterData data = new GetINDMasterData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspManageIndicatorsMasterData");
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);
            if (obj.PreparationMasterID > default(int))
                ctx.AddInParameter<int>(cmd, "@PreparationMasterID", obj.PreparationMasterID);
            if (obj.PreparationTypeID > default(int))
                ctx.AddInParameter<int>(cmd, "@PreparationTypeID", obj.PreparationTypeID);
            if (obj.SolutionID > default(int))
                ctx.AddInParameter<int>(cmd, "@SolutionID", obj.SolutionID);
            if (!string.IsNullOrEmpty(obj.Description))
                ctx.AddInParameter<string>(cmd, "@Description", obj.Description);
            if (obj.EntActID > default(int))
                ctx.AddInParameter<int>(cmd, "@EntActID",obj.EntActID);
            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);
            ctx.AddInParameter<int>(cmd, "@UserRoleID", tr.UserRoleID);
            ctx.AddInParameter<int>(cmd, "@DeptID", tr.DeptID);
            ctx.AddInParameter<short>(cmd, "@PlantID", tr.PlantID);
            ctx.AddOutParameter(cmd, "@RetMsg", System.Data.DbType.String, 25);
            using (var reader = cmd.ExecuteReader())
            {
                var rrRes = ((IObjectContextAdapter)context).ObjectContext.Translate<GetINDMasterData>(reader);
                foreach (var rr in rrRes)
                    data = rr;

            }
            data.ReturnFlag = ctx.GetOutputParameterValue(cmd, "@RetMsg");
            cmd.Connection.Close();
            ctx.CloseConnection(context);
            return data;
        }

        public VolumetricSolIndexData ManageTestSolutionIndex(VolumetricSolIndexFilter obj, TransResults trn)
        {
            var lst = new VolumetricSolIndexData();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspINDManageTestSoultionIndex");
            ctx.AddInParameter<short>(cmd, "@PlantID", trn.PlantID);
            ctx.AddInParameter<string>(cmd, "@Type", obj.Type);

            if (obj.IndexID > default(short))
                ctx.AddInParameter<short>(cmd, "@IndexID", obj.IndexID);

            if (obj.MaterialID > default(int))
                ctx.AddInParameter<int>(cmd, "@MaterialID", obj.MaterialID);

            if (!string.IsNullOrEmpty(obj.StpRefNumber))
                ctx.AddInParameter<string>(cmd, "@StpRefNumber", obj.StpRefNumber);

            if (!string.IsNullOrEmpty(obj.Comments))
                ctx.AddInParameter<string>(cmd, "@Comments", obj.Comments);
            if(!string.IsNullOrEmpty(obj.Status))
                ctx.AddInParameter<string>(cmd, "@Status",obj.Status);

            ctx.AddInParameter<string>(cmd, "@EntityCode", obj.EntityCode);

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

        public GetVolumetricSolIndex GetTestSolutionIndexByID(short indexID)
        {
            var lst = new GetVolumetricSolIndex();

            var cmd = ctx.PrepareCommand(context);
            ctx.PrepareProcedure(cmd, "solmgmt.uspINDGetTestSolutionIndexByID");
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

    }
}
