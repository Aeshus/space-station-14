namespace Content.Client.StyleProto;

public interface ISheetletFactory
{
    void Initialize();

    T GetConfig<T>()
        where T : SheetletConfig;

    SheetletConfig GetConfig(string name);
    T GetSheetlet<T>() where T : ISheetlet;
    ISheetlet GetSheetlet(string name);
}
