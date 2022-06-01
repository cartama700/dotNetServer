namespace Share.Master
{
    public interface IMasterBase
    {
        /// <summary>
        /// 고유 아이디
        /// </summary>
        public uint Id { get; set; }

        /// <summary>
        /// 이름
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 사용여부
        /// </summary>
        public bool IsUse { get; set; }
    }
}
