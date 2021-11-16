import { Component } from "react";
import '../css/relatorio.css'
import EnfeiteTelas from '../components/enfeiteTela';
import BarraLateral from '../components/barra'

class relatorios extends Component {

    constructor(props) {
        super(props);
        this.state = {
            logout: false
        }
    }




    render() {
        return (
            <section>

                <section className="corpoRelatorio">
                    <div>
                        <select className="selectRelatorio">
                            <option>Teste</option>
                            <option>Teste</option>
                            <option>Teste</option>
                            <option>Teste</option>
                            <option>Teste</option>
                        </select>

                        <table className="tabelaRelatorio">
                            <div className="tituloTabelaRelatorio">
                                <h4>Relatório</h4>
                                <div className='subtitulosTabelaRelatorio'>
                                    <h5>Data</h5>
                                    <h5>Valor</h5>
                                </div>
                            </div>

                            <div className="linhaTabelaRelatorio">
                                <h2>Conta de luz</h2>
                                <div className='subLinhaTabelaRelatorio'>
                                    <h5>30/06/2003</h5>
                                    <h5>R$200,00</h5>
                                </div>
                            </div>
                            <div className="linhaTabelaRelatorio">
                                <h2>Conta de luzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzzz</h2>
                                <div className='subLinhaTabelaRelatorio'>
                                    <h5>30/06/2003</h5>
                                    <h5>R$200,00</h5>
                                </div>
                            </div>
                        </table>

                        <button className='buttonDownRelatorio'>Fazer dowload do relatório</button>
                    </div>
                </section>

                <EnfeiteTelas />

                <BarraLateral />


            </section>
        )
    }
}

export default relatorios