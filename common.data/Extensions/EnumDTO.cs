using System;
namespace common.data.Extensions
{
    public class EnumDTO
    {
        public int Key { get { return Convert.ToInt32(_enum); } }
        public string Name { get { return _enum.ToString(); } }
        private Enum _enum;
        public EnumDTO(Enum inputEnum)
        {
            _enum = inputEnum;
        }
    }
}

