import React, { useState } from 'react'
import styles from './Main.module.scss';

const Filters = ({showFilters}) => {
    return (
        <div className={styles.modal}>
            <img onClick={showFilters} className={styles.close}width={35} src='./img/close.png' />
            <ul>
                <li>
                    <div>
                        <p>Enter salary:</p>
                    </div>

                    <input placeholder='Minimum salary' />
                    <input placeholder='Maximum salary' />
                </li>
                <li>
                    <p>Enter date of creation:</p>
                    <input type='date' />
                </li>
                <li>
                    <p>Enter skills</p>

                </li>
                <li>
                    Enter location
                </li>
            </ul>
        </div>
    )
}

export default Filters
