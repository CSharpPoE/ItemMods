
namespace ItemMods.Model.FutureStuff
{
    interface IMod
    {
        BaseMods BaseMods { get; set; }
        EssenceMods EssenceMods { get; set; }
        CorruptedMods CorruptedMods { get; set; }
        ElderMods ElderMods { get; set; }
        ShaperMods ShaperMods { get; set; }
        MasterMods MasterMods { get; set; }
        SignatureMods SignatureMods { get; set; }
    }
}