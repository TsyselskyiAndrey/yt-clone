import './Explore.css';
import { useEffect } from 'react';
import useCategories from "../../hooks/useCategories"
import { useNavigate } from 'react-router-dom';
export default function Explore() {
  const { categories, GetCategories } = useCategories();
  const navigate = useNavigate();
  useEffect(() => {
    GetCategories();
  }, [])
  return (
    <>
      <div className="videosContainer">
        <div className="video-grid">
          {
            Object.keys(categories).map((categoryIdx) => {
              return (
                <div className={"card " + categories[categoryIdx].name.toLowerCase()} key={categories[categoryIdx].id} onClick={() => navigate(`${categories[categoryIdx].id}`)}>
                  <h2 className="category-title">{categories[categoryIdx].name}</h2>
                  <div className="card-info">
                    <p>🎥 <span className="info-value">{categories[categoryIdx].numberOfVideos}</span> videos</p>
                    <p>👁️ <span className="info-value">{categories[categoryIdx].numberOfViews}</span> views</p>
                  </div>
                </div>

              )
            })
          }
        </div>
      </div>
    </>
  );
}