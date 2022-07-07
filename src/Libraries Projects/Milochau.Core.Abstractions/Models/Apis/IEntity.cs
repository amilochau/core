namespace Milochau.Finance.Helpers
{
    /// <summary>Entity</summary>
    /// <typeparam name="TKey">Type of key for object id</typeparam>
    public interface IEntity<TKey>
    {
        /// <summary>Id</summary>
        TKey Id { get; set; }
    }
}
