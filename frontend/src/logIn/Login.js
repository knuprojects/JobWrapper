import { useState } from "react"
import { useNavigate } from "react-router-dom";
import styles from './LogIn.module.scss'
import { Link } from 'react-router-dom'
import { toast, ToastContainer } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';

function LogIn() {
    
    const navigate = useNavigate();
    const [userName, setUserName] = useState('');
    const [password, setPassword] = useState('');

    function getUserName(event) {
        setUserName(event.target.value);
    }

    function getPassword(event) {
        setPassword(event.target.value);
    }

    const checkOut = () => {
        let check = true;
        if (userName === '1' || userName === null) {
            check = false;
            toast.warning('Enter something in username');
        }
        if (password === '' || password === null) {
            toast.warning('Enter something in password');
            check = false;
        }
        return check;
    }

    const prevDef = (e) => {
        e.preventDefault();
        const formData = new URLSearchParams();
        formData.append('userName', userName);
        formData.append('password', password);
        if (checkOut()) {
            fetch('http://localhost:5010/api/users/sign-in', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify({ userName, password })
            })
                .then(async (response) => {
                    const resp = await response.json();
                    if (response.status === 200) {
                        localStorage.setItem('token', resp.token);
                        navigate('/main');
                    }
                    console.log(resp);
                })
                .catch((error) => {
                    console.error('Error:', error);
                });
        }
    }

    return (
        <div className="clear">
            <div className={styles.wrapper}>
                <div className={styles.login} >
                    <div className={styles.inputs}>
                        <h4>Sign In</h4>
                        <form onSubmit={prevDef}>
                            <ul>
                                <li>
                                    <input value={userName}
                                        onChange={getUserName}
                                        className={styles.input}
                                        placeholder="Username" />
                                </li>
                                <li className="mt-20">
                                    <input value={password}
                                        onChange={getPassword}
                                        className={styles.input}
                                        placeholder="password"
                                        type='password' />
                                </li>
                                <div className={styles.container}>
                                    <li className="mt-30">
                                        <button onClick={checkOut}
                                            className={styles.button}>Submit</button>
                                    </li>
                                </div>

                                <li>
                                    <div className={styles.registration}>
                                        <p>Didn't have an account?</p>
                                        <Link to='/registration'> <p className={styles.link}>Sign-up</p></Link>
                                    </div>
                                </li>
                            </ul>
                        </form>
                    </div>
                    <img className={styles.img} src='/img/OrgCoral_Ofc-02_Concept-04.jpg' alt='picture' />
                </div>
            </div>
            <ToastContainer autoClose={4000} position='top-center' />
        </div>
    )
}

export default LogIn;
