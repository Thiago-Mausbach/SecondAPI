import './App.css';
import 'bootstrap/dist/css/bootstrap.min.css';
import {Modal, ModalBody, ModalFooter, ModalHeader} from 'reactstrap';
import { BrowserRouter, Routes, Route, Link, Navigate, useNavigate } from 'react-router-dom';
import React, { useState, useEffect } from 'react';
import axios from 'axios';



function ProtectedRoute({ children }) {
    const token = localStorage.getItem("Token");
    if (!token) {
        return <Navigate to="/" replace />;
    }

    return children;
}

axios.interceptors.request.use((config) => {
    const token = localStorage.getItem("Token");
    if (token) {
        config.headers.Authorization = `Bearer ${token}`;
    }
    return config;
});

function Home() {
    const baseUrl = "https://localhost:7146/API/Auth";

    const navigate = useNavigate();

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

            localStorage.setItem("Token", response.data.token);
            console.log(response.data.token);

            setMensagem("Login realizado com sucesso!");

            setTimeout(() => navigate("/Livros"), 1000);
        } catch (error) {
            setMensagem("Usuário ou senha incorretos.");
            console.error("Erro Login:", error);
        }
    };

    const requestRegister = async () => {
        try {
            const response = await axios.post(`https://localhost:7146/API/Usuarios`, {
                email: usuario.email,
                senha: usuario.senha,
                nome: usuario.nome,
                sobrenome: usuario.sobrenome
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
                        <button type="button" onClick={() => { setIsLogin(false); setMensagem(""); }} >
                            Cadastre-se
                        </button>
                    </>
                ) : (
                    <>
                        Já tem conta?{" "}
                        <button type="button" onClick={() => { setIsLogin(true); setMensagem(""); }} >
                            Faça login
                        </button>
                    </>
                )}
            </p>
            {mensagem && <p>{mensagem}</p>}
        </div>
    );
}


function Emprestimos() {
    const baseUrl = "https://localhost:7146/API/Emprestimo";

    const [data, setData] = useState([])

    const [modalIncluir, setModalIncluir] = useState(false);

    const [modalEditar, setModalEditar] = useState(false);

    const [modalExcluir, setModalExcluir] = useState(false);

    const [emprestimoSelecionado, setEmprestimoSelecionado] = useState({
        usuarioEmail: '',
        livroTitulo: '',
    })

    const handleChange = e => {
        const { name, value } = e.target;
        setEmprestimoSelecionado({
            ...emprestimoSelecionado, [name]: value
        });
        console.log(emprestimoSelecionado);
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

    const selecionarEmprestimo = (emprestimo, caso) => {
        setEmprestimoSelecionado(emprestimo);
        (caso === "Editar") ?
            abrirFecharModalEditar() : abrirFecharModalExcluir();
    }

    const requestGet = async () => {
        const token = localStorage.getItem("Token");
        await axios.get(baseUrl, {
            headers: {
                Authorization: `Bearer ${token}`
            }
        })
            .then(response => {
                setData(response.data);
            }).catch(error => {
                console.log(error);
            })
    }

    const requestPost = async () => {
        await axios.post(baseUrl, [emprestimoSelecionado])
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
            ...emprestimoSelecionado
        };

        try {
            await axios.put(`${baseUrl}/${emprestimoSelecionado.id}`, payload);

            setData(prev =>
                prev.map(emprestimo =>
                    emprestimo.id === payload.id ? { ...payload } : emprestimo
                )
            );

            abrirFecharModalEditar();
        } catch (error) {
            console.error("Erro no PUT:", error);
        }
    };

    const pedidoDelete = async () => {
        await axios.delete(baseUrl + "/" + emprestimoSelecionado.id)
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
        <div>
            <br></br>
            <h3>Registo de empréstimos</h3>
            <header>
                <button onClick={() => abrirFecharModalIncluir()} className="btn btn-success">Incluir novo Empréstimo</button>
            </header>
            <br></br>
            <table className="table table-bordered" >

                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Usuário</th>
                        <th>Livro</th>
                        <th>Data do empréstimo</th>
                        <th>Data de devolução</th>
                        <th>Ações</th>
                    </tr>
                </thead>
                <tbody>

                    {data.map(emprestimo => (
                        <tr key={emprestimo.id}>
                            <td>{emprestimo.id}</td>
                            <td>{emprestimo.usuarioEmail || "---"}</td>
                            <td>{emprestimo.livroTitulo || "---"}</td>
                            <td>{new Date(emprestimo.dataEmprestimo).toLocaleString("pt-BR")}</td>
                            <td>{new Date(emprestimo.dataDevolucao).toLocaleString("pt-BR")}</td>
                            <td>
                                <button className="btn btn-primary" onClick={() => selecionarEmprestimo(emprestimo, "Editar")}>Editar</button> {" "}
                                <button className="btn btn-danger" onClick={() => selecionarEmprestimo(emprestimo, "Excluir")}>Excluir</button> {" "}
                            </td>
                        </tr>
                    ))}
                </tbody>
            </table>

            <Modal isOpen={modalIncluir}>
                <ModalHeader>Incluir Emprestimos</ModalHeader>
                <ModalBody>
                    <div className="form-group">
                        <label>Livro: </label>
                        <br />
                        <input type="text" className="form-control" name='livroTitulo' onChange={handleChange} />
                        <br />
                        <label>Usuário: </label>
                        <br />
                        <input type="text" className="form-control" name='usuarioEmail' onChange={handleChange} />
                        <br />
                        {/*<label>Data do empréstimo: </label>*/}
                        {/*<br />*/}
                        {/*<input type="text" className="form-control" name='dataEmprestimo' onChange={handleChange} />*/}
                        {/*<br />*/}
                    </div>
                </ModalBody>
                <ModalFooter>
                    <button className="btn btn-primary" onClick={() => requestPost()}>Incluir</button>{"   "}
                    <button className="btn btn-danger" onClick={() => abrirFecharModalIncluir()}>Cancelar</button>
                </ModalFooter>
            </Modal>

            <Modal isOpen={modalEditar}>
                <ModalHeader>Editar empréstimo</ModalHeader>
                <ModalBody>
                    <div className="form-group">
                        <label>ID: </label>
                        <br />
                        <input type="text" className="form-control" value={emprestimoSelecionado.id} readOnly />
                        <br />
                        <label>Livro: </label>
                        <br />
                        <input type="text" className="form-control" name="titulo" onChange={handleChange} value={emprestimoSelecionado.titulo} />
                        <br />
                        <label>Usuário: </label>
                        <br />
                        <input type="text" className="form-control" name="autor" onChange={handleChange} value={emprestimoSelecionado.autor} />
                        <br />
                        <label>Data do emprestimo: </label>
                        <br />
                        <input type="text" className="form-control" name="ano" onChange={handleChange} value={emprestimoSelecionado.ano} />
                        <br />
                        <label>Data da devolução: </label>
                        <br />
                        <input type="text" className="form-control" name="genero" onChange={handleChange} value={emprestimoSelecionado.genero} />
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
                    Deseja excluir este registro?: {emprestimoSelecionado && emprestimoSelecionado.id}?
                </ModalBody>
                <ModalFooter>
                    <button className="btn btn-danger" onClick={() => pedidoDelete()}> Sim</button>
                    <button className="btn btn-secondary" onClick={() => abrirFecharModalExcluir()}>Cancelar</button>
                </ModalFooter>
            </Modal>

        </div>
    );
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
        await axios.delete(baseUrl + "/" + livroSelecionado.id)
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
            <nav>
                <Link to="/">Login</Link> |{" "}
                <Link to="/Livros">Livros</Link> |{" "}
                <Link to="/Emprestimos">Empréstimos</Link>{ " "}
            </nav>

            <Routes>
                <Route path="/" element={<Home />} />
                <Route path="/Livros" element={<ProtectedRoute><ListaLivros /></ProtectedRoute>} />
                <Route path="/Emprestimos" element={<ProtectedRoute><Emprestimos /></ProtectedRoute>} />
            </Routes>
        </BrowserRouter>
    );
}

export default App;