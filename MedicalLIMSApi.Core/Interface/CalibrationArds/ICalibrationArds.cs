using MedicalLIMSApi.Core.Entities.CalibrationArds;
using MedicalLIMSApi.Core.Entities.Common;
using System.Collections.Generic;

namespace MedicalLIMSApi.Core.Interface.CalibrationArds
{
    public interface ICalibrationArds
    {
        CalibrationArdsHeader GetCalibrationArdsHeaderInfo(int ID, int userRoleID);

        SearchResults<GetEquipmentMaintenance> SearchEquipmentMaintenance(SearchEquipmentMaintenance obj, short plantID);

        List<GetEquipmentCategories> GetEquipmentCategories(string deptCode);

        List<GetEquipmentTypesByCategory> GetEquipmentTypesByCategory(EquipmentTypesByCategory obj);

        List<GetScheduleTypesByDeptCode> GetScheduleTypesByDeptCode(string deptCode);

        RecordActionDetails RunCalibration(RunCalibration obj, TransResults tr);

        RecordActionDetails GenerateNewRequest(EQUPMAINInsertNewRequest obj, TransResults tr);
    }
}
