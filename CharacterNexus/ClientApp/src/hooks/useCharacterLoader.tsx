import { useState, useEffect, useRef } from "react";
import { UseFormReset, UseFormSetValue } from "react-hook-form";
import { useAppDispatch, useAppSelector } from "../store/configureStore";
import { fetchCharacterSchema, fetchCharacterByName } from "../store/slices/characterSlice";
import { getFileNameFromUrl } from "../utils/getFileNameFromUrl";
import { getURLParameter } from "../utils/getUrlParameter";

export function useCharacterLoader(
  reset: UseFormReset<any>,
  setValue: UseFormSetValue<any>,
  onCharacterLoaded?: () => void
) {
  const dispatch = useAppDispatch();
  const { currentRuleset } = useAppSelector((state: { ruleset: any }) => state.ruleset);
  const { schema, currentCharacter } = useAppSelector((state: { character: any }) => state.character);

  const [imagePreview, setImagePreview] = useState<string | null>(null);
  const [imageData, setImageData] = useState<File | null>(null);

  // Keep the latest callback without re-running the load effect when it changes
  const onLoadedRef = useRef(onCharacterLoaded);
  onLoadedRef.current = onCharacterLoaded;

  // Load schema on ruleset change
  useEffect(() => {
    if (!currentRuleset) return;
    dispatch(fetchCharacterSchema(currentRuleset.name));
  }, [currentRuleset, dispatch]);

  // Load character if query param exists
  useEffect(() => {
    if (!currentRuleset || !schema) return;

    const characterName = getURLParameter("character");
    if (characterName) {
      dispatch(fetchCharacterByName({ rulesetName: currentRuleset.name, characterName }));
    }
  }, [currentRuleset, schema, dispatch]);

  // When currentCharacter loads, reset form and load image
  useEffect(() => {
    if (!currentCharacter) return;

    reset(currentCharacter);
    onLoadedRef.current?.();

    if (currentCharacter.image) {
      setImagePreview(currentCharacter.image);
      fetch(currentCharacter.image)
        .then((res) => res.blob())
        .then((blob) => {
          const file = new File([blob], getFileNameFromUrl(currentCharacter.image), { type: blob.type });
          setValue("image", file, { shouldDirty: true });
          setImageData(file);
        });
    }
  }, [currentCharacter, reset, setValue]);

  return { imagePreview, imageData, setImagePreview, setImageData };
}