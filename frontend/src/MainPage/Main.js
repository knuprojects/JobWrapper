import React, { useState }  from 'react'
import Header from './Header';
function Main() {
   
    return (
        <div>
            <header className='d-flex'>
                <Header/>
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