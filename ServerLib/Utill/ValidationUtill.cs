namespace ServerLib.Utill
{
    /// <summary>
    /// 유효성 체크 유틸
    /// </summary>
    public static class ValidationUtill
    {
        public static List<string> ValidationSplit(this string str, char separator)
        {
            var result = str.Split(separator);
            if (result.Length == 0)
            {
                throw new ArgumentException("잘못된 요청 입니다.");
            }

            return result.ToList();
        }

        public static void ValidationIsDefined<TEnum>(this Enum @type, TEnum value)
        {
            if (value == null)
            {
                throw new ArgumentException("잘못된 요청 입니다.");
            }

            var result = Enum.IsDefined(typeof(TEnum), value);
            if (result == false)
            {
                throw new ArgumentException("잘못된 요청 입니다.");
            }
        }
    }
}
