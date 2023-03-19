import { useState } from "react"
import styles from './LogIn.module.scss'
import { Route, Routes, Link } from 'react-router-dom'
import Registration from "../Registration/Registration";

function LogIn() {
    const [logIn, setLogin] = useState(false);
    return (
        <div className="clear">
            <div className={styles.wrapper}>
                <div className={styles.login} >
                    <div className={styles.inputs}>
                        <h4>Sign In</h4>
                        <ul>
                            <li>
                                <input className={styles.input} placeholder="Username" />
                            </li>
                            <li className="mt-20">
                                <input className={styles.input} placeholder="password" type='password' />
                            </li>
                            <div className={styles.container}>
                                <li className="mt-30">
                                    <button className={styles.button}>Submit</button>
                                </li>
                            </div>

                            <li>
                                <div className={styles.registration}>
                                    <p>Didn't have an account?</p>
                                    <Link to='/registration'> <p className={styles.link}>Sign-up</p></Link>
                                </div>
                            </li>
                        </ul>
                    </div>
                    <img className={styles.img} src='/img/OrgCoral_Ofc-02_Concept-04.jpg' alt='picture' />
                </div>
            </div>
        </div>
    )
}

export default LogIn;