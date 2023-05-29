import React, { useEffect } from 'react';
import * as maptilersdk from '@maptiler/sdk';
import styles from '../MainPage/Main.module.scss';


function Map({location}) {

  useEffect(() => {
    maptilersdk.config.apiKey = 'H2he3gVAuTjVermVNqo6';
    const map = new maptilersdk.Map({
      container: 'map',
      style: maptilersdk.MapStyle.STREETS,
      center: [30.4222701, 50.446638],
      zoom: 14,
    });

    // Create a marker at the center of the map
    const marker = document.createElement('div');
    marker.className = 'marker';
    new maptilersdk.Marker(marker)
      .setLngLat([30.4222701, 50.446638])
      .addTo(map);

    return () => {
      // remove the map instance on unmounting the component
    };
  }, []);

  return <div id="map"/>;
}

export default Map;
