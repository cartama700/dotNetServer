namespace Share.Type.Item
{
    /// <summary>
    /// 사용되는 재화 종류
    /// </summary>
    public enum CashType
    {
        None = 0,

        /// <summary>
        /// 일반 재화
        /// </summary>
        Gold,

        /// <summary>
        /// 구매 재화
        /// </summary>
        Ruby,

        /// <summary>
        /// 특수 재화
        /// </summary>
        Crystal,

        /// <summary>
        /// 마일리지
        /// </summary>
        Mileage,
    }
}
