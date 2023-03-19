import style from './Registration.module.scss'
import { Route, Routes, Link } from 'react-router-dom'

function Registration() {
    return (
        <div className='clear'>
            <div className={style.wrapper}>
                <Link to='/'>
                    <button className={style.goBack}>Go back</button>
                </Link>
                <div className={style.registration}>
                    <ul>
                        <li>
                            <input className={style.input} placeholder="Username" /> 
                        </li>
                        <li>
                            <input className={style.input} placeholder="Email" />
                        </li>
                        <li>
                            <input className={style.input} placeholder="Password" type='password' />
                        </li>
                        <li>
                            <input className={style.input} placeholder="Confirm password" type='password'/>
                        </li>
                        <li className="mt-40">
                            <button className={style.button}>Submit</button>
                        </li>
                    </ul>
                </div>
                <label className={style.subscribe}>
                    Send email about new vacancies which you interested i, if it possible
                    <input type="checkbox" />
                    <span class="checkmark" />
                </label>
            </div>
        </div>
    )
}

export default Registration;