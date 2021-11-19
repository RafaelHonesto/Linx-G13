import React, { Component } from "react";
import '../css/telaInicio.css';
import enfeiteModal from '../img/enfeiteModal.png'
import BarraLateral from '../components/barra'
import axios from "axios";
import { Link } from "react-router-dom";

var data = new Date();
var dia = String(data.getDate()).padStart(2, '0');
var mes = String(data.getMonth() + 1).padStart(2, '0');
var ano = data.getFullYear();

var dataAtual = dia + '/' + mes + '/' + ano;

class telaInicio extends Component {

    constructor(props) {
        super(props);
        this.state = {
            listaDespesas: [],
            listaValores: [],
            totalValorEntrada: 0,
            nomeFuncionario: [],
            totalValorSaida: 0,
            nomeSetor: [],
            vazio: false,
            vazio2: false,
            cor: false
        }
    }

    somarValor = () => {

        let numSaida = 0;
        let numEntrada = 0
        this.state.listaValores.forEach(element => {
            if (element.tipoEntrada) {
                numEntrada += parseInt(element.valor)

            } else (
                numSaida += parseInt(element.valor)
            )

        });

        this.setState({ totalValorEntrada: numEntrada })
        this.setState({ totalValorSaida: numSaida })
    }

    abreModal2 = () => {
        const modal = document.getElementById('modal2')
        modal.classList.add('mostrar')
        modal.addEventListener('click', (e) => {
            if (e.target.id === "modal2" || e.target.id === "fechar") {
                modal.classList.remove('mostrar')
            }
        })
    }

    listarDespesas = () => {
        axios('http://localhost:5000/api/Despesa', {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(response => {
                if (response.status === 200) {
                    this.setState({ listaDespesas: response.data })

                    console.log(this.state.listaDespesas)

                    if (response.data.length === 0) {
                        this.setState({ vazio2: true })
                    }
                }
            })

            .catch(erro => console.log(erro))
    }

    listarValores = async () => {
        await axios('http://localhost:5000/api/Valor', {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(response => {
                if (response.status === 200) {
                    this.setState({ listaValores: response.data, listaSoValores: response.data })

                    this.somarValor();

                    console.log(response.data)

                    if (response.data.length === 0) {
                        this.setState({ vazio: true })
                    }
                }
            })

            .catch(erro => {
                if (erro) {
                    this.setState({ vazio: true })
                    console.log(erro)
                }
            })
        console.log(this.state.listaValores)
    }

    buscarUsuario() {
        axios('http://localhost:5000/api/Usuario/Buscar', {
            headers: {
                'Authorization': 'Bearer ' + localStorage.getItem('token-login')
            }
        })

            .then(response => this.setState({ nomeFuncionario: response.data.nome, nomeSetor: response.data.idSetorNavigation.nome }))
    }

    componentDidMount() {
        this.listarDespesas();
        this.listarValores();
        this.buscarUsuario();
        this.select();
    }

    select() {

        let check = document.getElementById('caixa')
        let texto = document.getElementById('textoPendentes')
        let texto1 = document.getElementById('textoPendentes1')
        let texto2 = document.getElementById('textoPendentes2')
        let label = document.getElementById('LabelSelect')

        check.addEventListener('click', () => {
            check.classList.add('transicaoTexto')
            label.classList.add('transicaoTexto')
            texto.classList.add('transicaoTexto')
            texto1.classList.add('transicaoTexto')
            texto2.classList.add('transicaoTexto')
        })

    }

    render() {
        return (
            <section className='body'>
                <section className='corpoInicial'>
                    <div className='esquerdo'>
                        <div className='titulo-inicio'>
                            <p>Olá, {this.state.nomeFuncionario}</p>
                            <p>Setor: {this.state.nomeSetor}</p>
                        </div>

                        <div className='content-despesas'>

                            <table className="tabela-inicio">

                                <div className='content-tabela-despesas'>
                                    <h4>Despesas pendentes</h4>
                                    <h5>Data de vencimento</h5>
                                    <h5>Valores</h5>
                                </div>

                                <td className="vazioDespesas" hidden={this.state.vazio2 === true ? false : true}>Não há nenhuma despesa cadastrada nesse setor</td>
                                <Link to='/despesas'> <button className="buttonValoresVazio" hidden={this.state.vazio === true ? false : true}>Cadastrar despesas +</button> </Link>

                                {
                                    this.state.listaDespesas.map((dados) => {
                                        return (

                                            <div className='content-tabela-despesas2 '>
                                                <td>{dados.nome}</td>
                                                <td>{new Intl.DateTimeFormat('pt-BR').format(new Date(dados.dataDespesa))}</td>
                                                <td id='vermelhoTexto'>R${dados.valor}</td>
                                            </div>
                                        )
                                    })
                                }

                            </table>
                        </div>

                        <div className='content-despesas2'>
                            <table className="tabela-inicio">

                                <div className='content-tabela-despesas fluxo-tabela'>
                                    <h4>Fluxo de caixa</h4>
                                    <h5>Data</h5>
                                    <h5>Valores</h5>
                                </div>

                                <td className="vazioValores" hidden={this.state.vazio === true ? false : true}>Não há nenhum valor cadastrado nesse setor</td>
                                <Link to='/valores'> <button className="buttonValoresVazio" hidden={this.state.vazio === true ? false : true}>Cadastrar valores +</button> </Link>

                                {
                                    this.state.listaValores.map((dados) => {
                                        return (

                                            <div className='content-tabela-despesas3 '>
                                                <td>{dados.titulo}</td>
                                                <td>{new Intl.DateTimeFormat('pt-BR').format(new Date(dados.dataValor))}</td>
                                                <td id={dados.tipoEntrada === true ? 'verdeTexto' : 'vermelhoTexto'}>R$ {dados.valor}</td>
                                            </div>
                                        )
                                    })
                                }

                            </table>
                        </div>
                    </div>
                    <div className='direito'>

                        <div className='container-fluxo'>
                            <div className='content-despesas3'>
                                <div className='titulo-entradas'>
                                    <h4>Atualizar status das despesas ⚠️</h4>
                                </div>

                                <div className='contentCheckBox'>
                                    <p id='textoPendentes'>Energia</p>
                                    <p id='textoPendentes1'>14/05/2021</p>
                                    <p id='textoPendentes2'>R$200</p>
                                    <div className='checkboxPendentes'>
                                        <label id='LabelSelect' for="caixa">Pago</label>
                                        <input type="radio" id="caixa" name="caixa" onClick={this.select} />
                                    </div>
                                </div>
                                <div className='contentCheckBox'>
                                    <p id='textoPendentes'>Energia</p>
                                    <p id='textoPendentes'>14/05/2021</p>
                                    <p id='textoPendentes'>R$200</p>
                                    <div className='checkboxPendentes'>
                                        <label id='LabelSelect' for="caixa">Pago</label>
                                        <input type="radio" id="caixa" name="caixa" onClick={this.select} />
                                    </div>
                                </div>
                            </div>

                            <div className='content-despesas4'>
                                <div className='titulo-entradas'>
                                    <h4>Lucro</h4>
                                    <h5>{dataAtual}</h5>
                                </div>
                                <p className='p-entradas'>R$ {(this.state.totalValorEntrada - this.state.totalValorSaida)}</p>
                            </div>

                            <div className='content-despesas4'>
                                <div className='titulo-entradas'>
                                    <h4>Total de entradas</h4>
                                    <h5>{dataAtual}</h5>
                                </div>
                                <p className='p-entradas'>R$ {this.state.totalValorEntrada}</p>
                            </div>

                            <div className='content-despesas4'>
                                <div className='titulo-entradas'>
                                    <h4>Total de saídas</h4>
                                    <h5>{dataAtual}</h5>
                                </div>
                                <p className='p-saidas'>R$ {this.state.totalValorSaida}</p>
                            </div>

                        </div>
                    </div>
                </section>
                <div class="container">
                    <ul>
                        <li class="active">Início</li>
                        <li>Valores</li>
                        <li>Despesas</li>
                        <li>Dashboard</li>
                        <li>Perfil</li>
                        <li>Sair</li>
                    </ul>
                </div>


                <section id="modal" className="modal">
                    <div className="modal-containerAdm">
                        <img src={enfeiteModal} alt='enfeite modal' className="imagemEnfeiteModal" />
                        <div className="content-modal">
                            <div className="inputsPerfil">
                                <input placeholder="Nome do setor" type="text" />
                                <input placeholder="Nome do gestor" type="text" />
                                <input placeholder="CPF" type="text" />
                                <input placeholder="Acesso" type="text" />
                                <input placeholder="Senha de acesso" type="text" />
                                <button>Criar</button>
                            </div>
                        </div>
                    </div>
                </section>
                <section id="modal2" className="modal">
                    <div className="modal-containerAdm">
                        <img src={enfeiteModal} alt='enfeite modal' className="imagemEnfeiteModal" />
                        <div className="content-modal">
                            <div className="inputsPerfil">
                                <input placeholder="Nome do setor" type="text" />
                                <input placeholder="Nome do gestor" type="text" />
                                <input placeholder="CPF" type="text" />
                                <input placeholder="Acesso" type="text" />
                                <input placeholder="Senha de acesso" type="text" />
                                <button>Criar</button>
                            </div>
                        </div>
                    </div>
                </section>

                <BarraLateral />

            </section>
        )
    }
}

export default telaInicio;