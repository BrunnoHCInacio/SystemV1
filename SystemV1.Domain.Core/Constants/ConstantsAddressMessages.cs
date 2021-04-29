using System;
using System.Collections.Generic;
using System.Text;

namespace SystemV1.Domain.Core.Constants
{
    public class ConstantsAddressMessages
    {
        public const string StreetRequired = "O logradoudo é obrigatório.";

        public const string StreetLength2_100 = "O logradouro deve conter entre 2 e 100 caracteres.";

        public const string DistrictRequired = "O bairro é obrigatório.";

        public const string DistrictLenght2_50 = "O bairro deve conter entre 2 e 50 caracteres.";
    }
}