import React, { useState } from 'react'
import styles from './Main.module.scss';

const Filters = ({ showFilters }) => {
    return (
        <div className={styles.modal}>
            <img onClick={showFilters} className={styles.close} width={35} src='./img/close.png' />
            <ul>
                <li>
                    <div>
                        <p>Enter salary:</p>
                    </div>
                    <input className={styles.filtersInput} placeholder=' Minimum salary' />
                    <input className={styles.filtersInput} placeholder=' Maximum salary' />
                </li>
                <li>
                    <p>Enter date of creation:</p>
                    <input className={styles.filtersInput} type='date' />
                </li>
                <li>
                    <p>Enter skills</p>
                    <input className={styles.filtersInput} placeholder=' Your skill:' />
                </li>
                <button className={styles.ok}>Оk</button>

            </ul>
        </div>
    )
}

export default Filters
