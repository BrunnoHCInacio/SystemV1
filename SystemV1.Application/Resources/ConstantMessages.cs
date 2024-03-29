﻿namespace SystemV1.Application.Resources
{
    public static class ConstantMessages
    {
        public const string PeopleNameRequired_EN = @"Name is required.";
        public const string PeopleNameRequired_PT = @"O nome é obrigatório.";
        public const string PeopleNameLength100_PT = @"O nome deve conter entre 2 e 100 caracteres.";
        public const string PeopleNameLength100_EN = @"The name must contain between 0 and 100 characters.";

        public const string StreetRequiredt_PT = @"O logradouro é obrigatório";
        public const string StreetRequiredt_EN = @"";
        public const string StreetLength100_PT = @"O logradouro deve conter entre 0 e 100 caracteres.";
        public const string ZipCodeRequiredt_PT = @"O CEP é obrigatório";
        public const string ZipCodeLength_PT = @"O CEP deve conter 7 caracteres";

        public const string CityRequired_PT = @"A cidade é obrigatória";
        public const string CityNameLength_PT = @"O nome da cidade deve conter entre 2 e 100 caracteres.";

        public const string DistrictRequired_PT = @"O nome do bairro é obrigatório";
        public const string DistrictLength_PT = @"O nome do bairro deve conter entre 2 e 50 caracteres.";

        public const string CountryNameRequired_PT = @"O nome do país é obrigatório.";
        public const string CountryNameLengh_PT = @"O nome do país deve conter entre 2 e 100 caracteres.";
        public const string StateRequired_PT = @"O estado é obrigatório.";

        public const string TypeContactRequired_PT = @"O tipo de contato deve ser informado";
        public const string ProviderDocumentRequired_PT = @"O CNPJ é obrigatório";
        public const string ProviderDocumentLength_PT = @"O CNPJ deve conter 18 caracteres, incluso a máscara.";
        public const string ClientDocumentRequired_PT = @"O CPF é obrigatório";
        public const string ClientDocumentLength_PT = @"O CPF deve conter 11 caracteres";

        public const string ProductItemModelRequired_PT = @"O modelo é obrigatório.";
        public const string ProductItemModelLength_PT = @"O modelo deve conter entre 2 e 150 caracteres.";
        public const string ProductItemValueRequired_PT = @"O valor é obrigatório.";

        public const string StateNameRequired_Pt = "O nome do estado é obrigatório.";
        public const string StateNameLenght_Pt = "O nome do estado deve conter entre 2 e 100 catacteres.";

        public static string pageAndPageSizeRequired => "É necessário informar a página e a quantidade de itens por página";
        public static string ProviderNotFound => "Fornecedor não encontrato";
        public static string ClientNotFound => "Cliente não encontrado.";

        public const string PasswordNotEquals = "As senhas não conferem.";
        public const string FieldIsRequired = "O campo {0} é obrigatório.";
        public const string InvalidValueField = "O campo {0} está em formato inválido.";
        public const string InvalidLengthField = "O campo {0} precisa ter entre {2} e {1} caracter.";
    }
}