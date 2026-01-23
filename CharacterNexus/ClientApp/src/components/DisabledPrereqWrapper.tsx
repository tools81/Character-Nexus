import React, { useState } from 'react';

interface Prerequisite {
  name: string;
  type?: string;
  formula?: string;
}

interface Props {
  component: any;
  disabled?: boolean;
  children: React.ReactNode;
}

const boxStyle: React.CSSProperties = {
  position: 'absolute',
  zIndex: 1050,
  background: 'white',
  border: '1px solid rgba(0,0,0,0.15)',
  padding: '8px',
  borderRadius: 4,
  boxShadow: '0 2px 6px rgba(0,0,0,0.2)',
  maxWidth: 320,
};

const disabledOverlayStyle: React.CSSProperties = {
  pointerEvents: 'none',
  opacity: 0.6,
};

const wrapperStyle: React.CSSProperties = {
  position: 'relative',
  display: 'inline-block',
}

const DisabledPrereqWrapper: React.FC<Props> = ({ component, disabled, children }) => {
  const [show, setShow] = useState(false);

  if (!disabled || !component) return <>{children}</>;

  let prereqs: Prerequisite[] = [];
  try {
    if (component.prerequisites && component.prerequisites !== 'null' && component.prerequisites !== '') {
      prereqs = JSON.parse(component.prerequisites) as Prerequisite[];
    }
  } catch (e) {
    // ignore parse error
  }

  if (!prereqs || prereqs.length === 0) {
    return (
      <span
        style={{
          ...wrapperStyle,
          ...(disabled ? disabledOverlayStyle : {}),
        }}
        onMouseEnter={() => setShow(true)}
        onMouseLeave={() => setShow(false)}
      >
        {children}
      </span>
    );
  }

  return (
    <span style={wrapperStyle} onMouseEnter={() => setShow(true)} onMouseLeave={() => setShow(false)}>
      {children}
      {show && (
        <div style={{ ...boxStyle, top: '100%', left: 0, marginTop: 6 }}>
          <strong style={{color: '#DB1D1D'}}>Requirements</strong>
          <ul style={{ margin: '6px 0 0 0', paddingLeft: 16 }}>
            {prereqs.map((p, i) => (
              <li key={i}>
                {p.type && p.type !== '' ? `${p.type}.` : ''}{p.name} {p.formula ? `(${p.formula})` : ''}
              </li>
            ))}
          </ul>
        </div>
      )}
    </span>
  );
};

export default DisabledPrereqWrapper;
