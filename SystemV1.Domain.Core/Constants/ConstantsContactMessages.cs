using System;
using System.Collections.Generic;
using System.Text;

namespace SystemV1.Domain.Core.Constants
{
    public static class ConstantsContactMessages
    {
        public const string TypeContactRequired = "O tipo de contato deve ser informado.";
        public const string PhoneNumberRequired = "O telefone fixo é obrigatório.";
        public const string PhoneNumberMinLength = "O telefone fixo deve conter 8 caracteres.";
        public const string PhoneNumberMaxLength = "O telefone fixo deve conter 8 caracteres.";
        public const string DddRequired = "O DDD é obrigatório";
        public const string DddMinLength = "O DDD deve conter 2 caracteres.";
        public const string DddMaxLength = "O DDD deve conter até 3 caracteres.";

        public const string CellPhoneRequired = "O celular é obrigatório.";
        public const string CellPhoneMinLength = "O celular deve conter 8 caracteres.";
        public const string CellPhoneMaxLength = "O celular deve conter no máximo 9 caracteres.";

        public const string EmailRequired = "O email é obrigatório.";
        public const string EmailValid = "Informe um email válido.";
    }
}