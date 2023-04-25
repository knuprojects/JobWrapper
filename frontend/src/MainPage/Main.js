import React, { useState, useEffect } from 'react'
import styles from './Main.module.scss';
import Items from '../Items/Items';
import Filters from './Filters';
import Map, { NavigationControl, Marker } from 'react-map-gl';
import maplibregl from 'maplibre-gl';
import 'maplibre-gl/dist/maplibre-gl.css';
function Main() {
    const [items, setItems] = useState([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize, setPageSize] = useState(10);
    const [totalItems, setTotalItems] = useState(0);
    const [filters, setFilters] = useState(false);
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState(null);
    const url = 'http://localhost:5020/api';
    useEffect(() => {
        async function getVacancies() {
            setIsLoading(true);
            try {
                const response = await fetch(`${url}/vacancies?PageNumber=${pageNumber}&PageSize=${pageSize}`, {
                    headers: {
                        'Content-Type': 'application/json',
                        Authorization: `Bearer ${localStorage.getItem('token')}`,
                    },
                });
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                const data = await response.json();
                setItems(data);
                setTotalItems(data.length); // assuming data.length is the total number of items
                setIsLoading(false);
            } catch (error) {
                setError(error.message);
                setIsLoading(false);
            }
        }
        getVacancies();
    }, [pageNumber, pageSize]);


    function showFilters() {
        setFilters(!filters);
    }

    function handlePageClick(pageNumber) {
        setPageNumber(pageNumber);
        setIsLoading(true);

        fetch(`${url}/vacancies?PageNumber=${pageNumber}&PageSize=${pageSize}`, {
            headers: {
                'Content-Type': 'application/json',
                Authorization: `Bearer ${localStorage.getItem('token')}`,
            },
        })
            .then((response) => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then((data) => {
                setItems(data);
                setTotalItems(data.length); // assuming data.length is the total number of items
                setIsLoading(false);
            })
            .catch((error) => {
                setError(error.message);
                setIsLoading(false);
            });
    }



    const pageCount = Math.ceil(totalItems / pageSize);
    const visibleItemsStartIndex = (pageNumber - 1) * pageSize;
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

                <div className="page">

                </div>
                <div className={styles.main}>
                    <div className={styles.content}>
                        <nav className={styles.search}>
                            <input className={styles.input} placeholder="Search..." />
                            <img onClick={showFilters} className={styles.photo} src='./img/Rectangle 22.png' width={38} height={40} alt='filter' />
                        </nav>
                        <aside className={styles.aside}>
                            <div>
                                {visibleItems.map((item, index) => (
                                    <Items
                                        key={index}
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
                    <div className={styles.pagination}>
                        <button
                            disabled={pageNumber === 1}
                            onClick={() => handlePageClick(pageNumber - 1)}
                        >
                            Previous
                        </button>
                        {Array.from({ length: pageCount }, (_, i) => (
                            <button
                                key={i}
                                onClick={() => handlePageClick(i + 1)}
                                className={i + 1 === pageNumber ? styles.active : ''}
                            >
                                {i + 1}
                            </button>
                        ))}
                        <button
                            disabled={pageNumber === pageCount}
                            onClick={() => handlePageClick(pageNumber + 1)}
                        >
                            Next
                        </button>
                    </div>
                </div>
                <main className={styles.map}>
                    <Map mapLib={maplibregl}
                        initialViewState={{
                            longitude: 30.4222701,
                            latitude: 50.446638,
                            zoom: 14
                        }}
                        style={{ width: "100%", height: " 100%" }}
                        mapStyle="https://api.maptiler.com/maps/streets-v2/style.json?key=H2he3gVAuTjVermVNqo6	"
                    >
                        <NavigationControl position="top-right" />
                        {items.map((item, index) => {
                            const [lat, lng] = item.location.split('&');
                            return (
                                <Marker
                                    key={index}
                                    longitude={parseFloat(lng)}
                                    latitude={parseFloat(lat)}
                                    color="#61dbfb"
                                />
                            );
                        })}
                    </Map>

                </main>

            </div>

        </div >

    );

}

export default Main;
