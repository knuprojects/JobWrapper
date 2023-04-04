import React, { useState } from 'react'
import Header from './Header';
import styles from './Main.module.scss';
function Main(props) {
    return (
        <div className={styles.main}>
            <header className={styles.header}>
                <Header />
            </header>
            <div className={styles.content}>
                <nav className={styles.nav}>
                    <input  placeholder="Search..."></input>
                </nav>
                <aside className={styles.search}>
                    Here be search
                </aside >
                <main className={styles.map}>
                    Map
                </main>
            </div>


        </div>
    )
}

export default Main;