import React from "react";
import { useLocation, Location } from "react-router-dom";

interface WithLocationProps {
  location: Location;
}

export function withLocation<T extends WithLocationProps>(
  Component: React.ComponentType<T>
) {
  return (props: Omit<T, keyof WithLocationProps>) => {
    const location = useLocation();
    return <Component {...(props as T)} location={location} />;
  };
}
