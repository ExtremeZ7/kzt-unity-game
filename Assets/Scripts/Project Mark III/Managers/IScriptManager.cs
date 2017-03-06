interface IScriptManager<T>
{
    void Register(T script);

    void Unregister(T script);
}
