using API_Simulacao_Hack.DTO;
using FluentValidation;

namespace API_Simulacao_Hack.Validators
{
    public class SolicitacaoSimulacaoValidator : AbstractValidator<SolicitacaoSimulacaoDTO>
    {
        public SolicitacaoSimulacaoValidator()
        {
            RuleFor(s => s.ValorDesejado)
                .GreaterThan(0)
                .WithMessage("O valor desejado deve ser maior que zero.");

            RuleFor(s => s.Prazo)
                .GreaterThan(0)
                .WithMessage("O prazo deve ser maior que zero.");
        }
    }
}
