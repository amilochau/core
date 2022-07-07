namespace Milochau.Core.Abstractions.Models.Apis
{
    /// <summary>Create response</summary>
    /// <typeparam name="TKey">Type of key for object id</typeparam>
    public interface ICreateResponse<TKey> where TKey : notnull
    {
        /// <summary>Id</summary>
        TKey Id { get; set; }
    }
}
