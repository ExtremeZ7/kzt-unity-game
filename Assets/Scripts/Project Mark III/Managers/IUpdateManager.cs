interface IUpdateManager<T>
{
    void Register(T script);

    void Unregister(T script);
}
