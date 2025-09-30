import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import {Modal, ModalBody, ModalFooter, ModalHeader} from 'reactstrap';
import { BrowserRouter, Routes, Route, Link } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import axios from 'axios';


function Home() {
    const baseUrl = "https://localhost:7146/API/Auth";

    const [usuario, setUsuario] = useState({
        email: "",
        senha: "",
        nome: "",
        sobrenome: ""
    });

    const [mensagem, setMensagem] = useState("");
    const [isLogin, setIsLogin] = useState(true);

    const handleChange = (e) => {
        const { name, value } = e.target;
        setUsuario((user) => ({
            ...user,
            [name]: value
        }));
    };

    const requestLogin = async () => {
        try {
            const response = await axios.post(`${baseUrl}/login`, {
                email: usuario.email,
                senha: usuario.senha
            });

            const { token } = response.data;
            localStorage.setItem("authToken", token);

            setMensagem("Login realizado com sucesso!");
        } catch (error) {
            setMensagem("Falha ao autenticar. Verifique email e senha.");
            console.error("Erro Login:", error);
        }
    };

    const requestRegister = async () => {
        try {
            const response = await axios.post(`https://localhost:7146/API/Usuarios`, {
                email: usuario.email,
                senha: usuario.senha,
                nome: usuario.nome,
                sobrenome: ususario.sobrenome
            });

            setMensagem("Cadastro realizado com sucesso! Agora faça login.");
            console.log("Resposta cadastro:", response.data);
        } catch (error) {
            setMensagem("Erro ao cadastrar. Tente novamente.");
            console.error("Erro Register:", error);
        }
    };

    const handleSubmit = (e) => {
        e.preventDefault();
        if (isLogin) {
            requestLogin();
        } else {
            requestRegister();
        }
    };

    return (
        <div style={{ maxWidth: "400px", margin: "50px auto" }}>
            <h2>{isLogin ? "Login" : "Cadastro"}</h2>
            <form onSubmit={handleSubmit}>
                {!isLogin && (
                    <>
                    <div>
                        <input type="text" name="nome" placeholder="Nome" value={usuario.nome} onChange={handleChange} required />
                    </div>
                     <div>
                        <input type="text" name="sobrenome" placeholder="Sobrenome" value={usuario.sobrenome} onChange={handleChange} />
                    </div>
                    </>
                )}
                <div>
                    <input type="email" name="email" placeholder="Email" value={usuario.email} onChange={handleChange} required />
                </div>
                <div>
                    <input type="password" name="senha" placeholder="Senha" value={usuario.senha} onChange={handleChange} required />
                </div>
                <button type="submit">
                    {isLogin ? "Entrar" : "Cadastrar"}
                </button>
            </form>
            <p style={{ marginTop: "10px" }}>
                {isLogin ? (
                    <>
                        Não tem conta?{" "}
                        <button type="button" onClick={() => setIsLogin(false)}>
                            Cadastre-se
                        </button>
                    </>
                ) : (
                    <>
                        Já tem conta?{" "}
                        <button type="button" onClick={() => setIsLogin(true)}>
                            Faça login
                        </button>
                    </>
                )}
            </p>
            {mensagem && <p>{mensagem}</p>}
        </div>
    );
}


function Contact() {
    return <h1>Contact Page</h1>;
}

function ListaLivros() {
    const baseUrl = "https://localhost:7146/API/Livros";

    const [data, setData] = useState([])

    const [modalIncluir, setModalIncluir] = useState(false);

    const [modalEditar, setModalEditar] = useState(false);

    const [modalExcluir, setModalExcluir] = useState(false);

    const [livroSelecionado, setLivroSelecionado] = useState({
        titulo: '',
        autor: '',
        ano: '',
        genero: ''
    })

    const handleChange = e => {
        const { name, value } = e.target;
        setLivroSelecionado({
            ...livroSelecionado, [name]: value
        });
        console.log(livroSelecionado);
    }

    const selecionarLivro = (livro, caso) => {
        setLivroSelecionado(livro);
        (caso === "Editar") ?
            abrirFecharModalEditar() : abrirFecharModalExcluir();
    }

    const abrirFecharModalEditar = () => {
        setModalEditar(!modalEditar);
    }

    const abrirFecharModalIncluir = () => {
        setModalIncluir(!modalIncluir);
    }

    const abrirFecharModalExcluir = () => {
        setModalExcluir(!modalExcluir);
    }


    const requestGet = async () => {
        await axios.get(baseUrl)
            .then(response => {
                setData(response.data);
            }).catch(error => {
                console.log(error);
            })
    }

    const requestPost = async () => {
        livroSelecionado.ano = parseInt(livroSelecionado.ano);
        await axios.post(baseUrl, [livroSelecionado])
            .then(response => {
                setData(data.concat(response.data));
                requestGet();
                abrirFecharModalIncluir();
            }).catch(error => {
                console.log(error);
            })
    }


    const requestPut = async () => {
        const payload = {
            ...livroSelecionado,
            ano: parseInt(livroSelecionado.ano)
        };

        try {
            await axios.put(`${baseUrl}/${livroSelecionado.id}`, payload);

            setData(prev =>
                prev.map(livro =>
                    livro.id === payload.id ? { ...payload } : livro
                )
            );

            abrirFecharModalEditar();
        } catch (error) {
            console.error("Erro no PUT:", error);
        }
    };

    const pedidoDelete = async () => {
        await axios.patch(baseUrl + "/" + livroSelecionado.id)
            .then(() => {
                requestGet();
                abrirFecharModalExcluir();
            }).catch(error => {
                console.log(error);
            })
    }

    useEffect(() => {
        requestGet();
    }, []);

    return (

        <div className="App">
            <br></br>
            <h3>Cadastro de livros</h3>
            <header>
                <button onClick={() => abrirFecharModalIncluir()} className="btn btn-success">Incluir novo livro</button>
            </header>
            <br></br>
            <table className="table table-bordered" >

                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Titulo</th>
                        <th>Autor</th>
                        <th>Ano</th>
                        <th>Genero</th>
                        <th>Acoes</th>
                    </tr>
                </thead>
                <tbody>

                    {data.map(livro => (
                        <tr key={livro.id}>
                            <td>{livro.id}</td>
                            <td>{livro.titulo}</td>
                            <td>{livro.autor}</td>
                            <td>{livro.ano}</td>
                            <td>{livro.genero}</td>
                            <td>
                                <button className="btn btn-primary" onClick={() => selecionarLivro(livro, "Editar")}>Editar</button> {" "}
                                <button className="btn btn-danger" onClick={() => selecionarLivro(livro, "Excluir")}>Excluir</button> {" "}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>


            <Modal isOpen={modalIncluir}>
                <ModalHeader>Incluir livros</ModalHeader>
                <ModalBody>
                    <div className="form-group">
                        <label>Titulo: </label>
                        <br />
                        <input type="text" className="form-control" name='titulo' onChange={handleChange} />
                        <br />
                        <label>Autor: </label>
                        <br />
                        <input type="text" className="form-control" name='autor' onChange={handleChange} />
                        <br />
                        <label>Ano: </label>
                        <br />
                        <input type="text" className="form-control" name='ano' onChange={handleChange} />
                        <br />
                        <label>Genero: </label>
                        <br />
                        <input type="text" className="form-control" name='genero' onChange={handleChange} />
                        <br />
                    </div>
                </ModalBody>
                <ModalFooter>
                    <button className="btn btn-primary" onClick={() => requestPost()}>Incluir</button>{"   "}
                    <button className="btn btn-danger" onClick={() => abrirFecharModalIncluir()}>Cancelar</button>
                </ModalFooter>
            </Modal>

            <Modal isOpen={modalEditar}>
                <ModalHeader>Editar Livro</ModalHeader>
                <ModalBody>
                    <div className="form-group">
                        <label>ID: </label>
                        <br />
                        <input type="text" className="form-control" value={livroSelecionado.id} readOnly />
                        <br />
                        <label>Titulo: </label>
                        <br />
                        <input type="text" className="form-control" name="titulo" onChange={handleChange} value={livroSelecionado.titulo} />
                        <br />
                        <label>Autor: </label>
                        <br />
                        <input type="text" className="form-control" name="autor" onChange={handleChange} value={livroSelecionado.autor} />
                        <br />
                        <label>Ano: </label>
                        <br />
                        <input type="text" className="form-control" name="ano" onChange={handleChange} value={livroSelecionado.ano} />
                        <br />
                        <label>Genero: </label>
                        <br />
                        <input type="text" className="form-control" name="genero" onChange={handleChange} value={livroSelecionado.genero} />
                        <br />
                    </div>
                </ModalBody>
                <ModalFooter>
                    <button className="btn btn-primary" onClick={() => requestPut()}>Editar</button>{"   "}
                    <button className="btn btn-danger" onClick={() => abrirFecharModalEditar()}>Cancelar</button>
                </ModalFooter>
            </Modal>

            <Modal isOpen={modalExcluir}>
                <ModalBody>
                    Deseja excluir este livro: {livroSelecionado && livroSelecionado.titulo}?
                </ModalBody>
                <ModalFooter>
                    <button className="btn btn-danger" onClick={() => pedidoDelete()}> Sim</button>
                    <button className="btn btn-secondary" onClick={() => abrirFecharModalExcluir()}>Cancelar</button>
                </ModalFooter>
            </Modal>

        </div>


    );
}


function App() {
    return (
        <BrowserRouter>
            {/* Navigation */}
            <nav>
                <Link to="/">Login</Link> |{" "}
                <Link to="/Livros">Livros</Link> |{" "}
                <Link to="/contact">Login</Link>
            </nav>

            {/* Routes */}
            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/Livros" element={<ListaLivros />} />
                <Route path="/contact" element={<Contact />} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;