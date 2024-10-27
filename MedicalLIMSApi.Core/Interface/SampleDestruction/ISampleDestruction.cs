using MedicalLIMSApi.Core.Entities.Common;
using MedicalLIMSApi.Core.Entities.SampleDestruction;

namespace MedicalLIMSApi.Core.Interface.SampleDestruction
{
    public interface ISampleDestruction
    {
        GetResultPacksForDestructionList GetPacksForDestruction(GetPacksForDestruction obj , short plantID );

        GetResultsDestructionSamplesDetails GetResultsDestructionSamples(int destructionID ,TransResults tr);

        GetPacksForDestructionDetails SavePacksForDestruction(SavePacksForDestruction obj, TransResults tr);

        RecordActionDetails ManageDestructionSamples(ManageDestructionSamples obj , TransResults tr);

        SearchResults<GetSampleDestruction> GetSampleDestruction(SearchSampleDestruction obj, short plantID);

        GetResultDiscardSample DiscardSamples(DiscardSamples obj, TransResults tr);

        GetSampleDestructionDetailsForView GetSampleDestructionDetailsForView(int destructionID);

    }
}
