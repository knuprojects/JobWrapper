import React, { useState, useEffect } from 'react'
import styles from './Main.module.scss';
import { data } from './Data.js';
import Items from '../Items/Items';
import Filters from './Filters';

function Main() {
    const [currentPage, setCurrentPage] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalItems, setTotalItems] = useState(0);
    const [items, setItems] = useState([]);
    const [filters, setFilters] = useState(false);

    useEffect(() => {
        // Your API call to fetch the data goes here
        // This is just an example using the data you provided
        setTotalItems(data[0].totalItems);
        setItems(data[0].items);
    }, []);

    function showFilters() {
        setFilters(!filters);
    }

    function handlePageClick(pageNumber) {
        setCurrentPage(pageNumber);
    }

    const pageCount = Math.ceil(totalItems / pageSize);
    const visibleItemsStartIndex = (currentPage - 1) * pageSize;
    const visibleItemsEndIndex = visibleItemsStartIndex + pageSize;
    const visibleItems = items.slice(visibleItemsStartIndex, visibleItemsEndIndex);

    return (
        <div className="clear">
            <div className={styles.wrapper}>
                {filters &&
                    <div className={styles.filters}>
                        <Filters showFilters={showFilters} />
                    </div>
                }
                <div className={styles.main}>
                    <div className={styles.content}>
                        <nav className={styles.search}>
                            <input className={styles.input} placeholder="Search..." />
                            <img onClick={showFilters} className={styles.photo} src='./img/Rectangle 22.png' width={38} height={40} alt='filter' />
                        </nav>
                        <aside className={styles.aside}>
                            <div>
                                {visibleItems.map((item) => (
                                    <Items
                                        key={item.gid}
                                        id={item.gid}
                                        name={item.name}
                                        skills={item.skills}
                                        location={item.location}
                                        salary={item.salary}
                                    />
                                ))}
                            </div>
                        </aside>
                    </div>
                    <main className={styles.map}>
                        <div className={styles.pagination}>
                            <button
                                disabled={currentPage === 1}
                                onClick={() => handlePageClick(currentPage - 1)}
                            >
                                Previous
                            </button>
                            {Array.from({ length: pageCount }, (_, i) => (
                                <button
                                    key={i}
                                    onClick={() => handlePageClick(i + 1)}
                                    className={i + 1 === currentPage ? styles.active : ''}
                                >
                                    {i + 1}
                                </button>
                            ))}
                            <button
                                disabled={currentPage === pageCount}
                                onClick={() => handlePageClick(currentPage + 1)}
                            >
                                Next
                            </button>
                        </div>
                    </main>
                </div>
            </div>
        </div>
    );
}

export default Main;
