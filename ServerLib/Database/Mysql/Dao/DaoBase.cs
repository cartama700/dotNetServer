namespace ServerLib.Database.Mysql.Dao
{
    internal interface DaoBase<T>
    {
        /// <summary>
        /// 추가를 하거나 이미 추가가 되었다면 업데이트를 진행
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<T> InsertOrUpdateAsync(T entity);

        /// <summary>
        /// 특정 컬럼만 업데이트 
        /// </summary>
        /// <param name="entity"></param>
        void Update(T entity);
    }
}
