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
            telemetria.TempoMedio = ((telemetria.TempoMedio * (telemetria.QtdRequisicoes - 1)) + tempoExecucao) / telemetria.QtdRequisicoes;
            telemetria.TempoMinimo = telemetria.QtdRequisicoes == 1 ? tempoExecucao : Math.Min(telemetria.TempoMinimo, tempoExecucao);
            telemetria.TempoMaximo = Math.Max(telemetria.TempoMaximo, tempoExecucao);

            if (sucesso)
            {
                telemetria.PercentualSucesso = ((telemetria.PercentualSucesso * (telemetria.QtdRequisicoes - 1)) + 100) / telemetria.QtdRequisicoes;
            }
            else
            {
                telemetria.PercentualSucesso = ((telemetria.PercentualSucesso * (telemetria.QtdRequisicoes - 1)) + 0) / telemetria.QtdRequisicoes;
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