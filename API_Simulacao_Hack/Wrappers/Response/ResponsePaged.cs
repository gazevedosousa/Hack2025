using System.Collections;


namespace API_Simulacao_Hack.Wrappers.Response;

public class ResponsePaged<T> {

    public ResponsePaged(List<T> registros, int pagina, int qtdRegistrosPagina, long qtdRegistros){
        this.registros = registros;
        this.pagina = pagina;
        this.qtdRegistros = qtdRegistros;
        this.qtdRegistrosPagina = qtdRegistrosPagina;
    }

    public int pagina {get; set;}
    public long qtdRegistros { get; set;}
    public int qtdRegistrosPagina {get; set;}
    public List<T> registros { get; set;}
}