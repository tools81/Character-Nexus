import React, { createContext, useContext, useState, useEffect, ReactNode, ReactElement } from 'react'

const SharedImageRetrievalContext = createContext<() => void>(() => {
  throw new Error(
    "SharedImageRetrieval must be used within a SharedImageRetrievalProvider"
  );
});

interface Props {
  image: string,
  children: ReactNode;
}

export const SharedImageRetrievalProvider: React.FC<Props> = ({
  image, children
}) => {
  const [imageUrl, setImageUrl] = useState("");

  var fetchUrl = "http://localhost:5000/api/image/" + {image};

  const imageRetrieval = (): ReactElement => {
    useEffect(() => {
      // Replace 'http://localhost:5000' with your API URL
      const fetchImage = async () => {
        const response = await fetch(
          fetchUrl
        );
        const blob = await response.blob();
        const url = URL.createObjectURL(blob);
        setImageUrl(url);
      };

      fetchImage();
    }, []);

    return <div>{imageUrl && <img src={imageUrl} alt="From API" />}</div>;
  };

  return (
    <SharedImageRetrievalContext.Provider value={imageRetrieval}>
      {children}
    </SharedImageRetrievalContext.Provider>
  );
};

export const useSharedFunction = (): (() => void) => {
  return useContext(SharedImageRetrievalContext);
};