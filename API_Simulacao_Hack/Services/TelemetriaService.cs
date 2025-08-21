using API_Simulacao_Hack.DTO;
using API_Simulacao_Hack.Interfaces.Services;
using API_Simulacao_Hack.Util;
using API_Simulacao_Hack.Util.Base;
using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
public class TelemetriaService: ITelemetriaService
{
    private readonly ConcurrentDictionary<string, TelemetriaDTO> _dados = new();

    public void RegistrarRequisicao(string nomeApi, double tempoExecucao, bool sucesso)
    {
        var telemetria = _dados.GetOrAdd(nomeApi, _ => new TelemetriaDTO { NomeApi = nomeApi });

        lock (telemetria)
        {
            telemetria.QtdRequisicoes++;
            telemetria.TempoMedio = Math.Round(((telemetria.TempoMedio * (telemetria.QtdRequisicoes - 1)) + tempoExecucao) / telemetria.QtdRequisicoes, 0);
            telemetria.TempoMinimo = Math.Round(telemetria.QtdRequisicoes == 1 ? tempoExecucao : Math.Min(telemetria.TempoMinimo, tempoExecucao), 0);
            telemetria.TempoMaximo = Math.Round(Math.Max(telemetria.TempoMaximo, tempoExecucao), 0);


            if (sucesso)
            {
                telemetria.PercentualSucesso = Math.Round(((telemetria.PercentualSucesso * (telemetria.QtdRequisicoes - 1)) + 1) / telemetria.QtdRequisicoes, 2);
            }
            else
            {
                telemetria.PercentualSucesso = Math.Round(((telemetria.PercentualSucesso * (telemetria.QtdRequisicoes - 1)) + 0) / telemetria.QtdRequisicoes, 2);
            }

        }
    }

    public ApiResponse<RetornoTelemetriaDTO> ObterTodasTelemetrias()
    {
        List<TelemetriaDTO> lsTelemetria = _dados.Values.ToList();

        RetornoTelemetriaDTO retorno = new RetornoTelemetriaDTO()
        {
            DataReferencia = DateOnly.FromDateTime(new DateTime().GetDataAtual().Date),
            ListaEndpoints = lsTelemetria
        };

        return ApiResponse<RetornoTelemetriaDTO>.SuccessOK(retorno);
    }
}