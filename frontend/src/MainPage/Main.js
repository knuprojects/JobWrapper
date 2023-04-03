import React, { useState } from 'react'
import Header from './Header';
import styles from './Main.module.scss';
import { useEffect } from 'react';
function Main(props) {

    
   return (
        <div>
            <header className={styles.header}>
                <Header />
            </header>
            <nav>
                Nav
            </nav>
            <aside>
                Here be search
            </aside>
            <main>
                Map
            </main>
        </div>
    )
}

export default Main;