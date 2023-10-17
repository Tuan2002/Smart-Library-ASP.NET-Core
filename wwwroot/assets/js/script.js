const currentLayout = localStorage.getItem("layout") || "light";
const currentMenu = localStorage.getItem("menu") || "side";

const customizerBtn = document.querySelector(".customizer-trigger");
const customizer = document.querySelector(".customizer-wrapper");
const customizerClose = document.querySelector(".customizer-close");
const customizerOverlay = document.querySelector(".customizer-overlay");

function closeCustomizer(e) {
    e.preventDefault();
    customizer.classList.remove("show");
    customizerBtn.classList.remove("show");
    customizerOverlay.classList.remove("show");
  }
function loadThemeFromLocal(){
    if (currentLayout === "dark") {
    const darkBanner = document.querySelector("#dark-banner-change");
    darkBanner?.classList?.add("active");
      $("body").removeClass("layout-light");
      $("body").addClass("layout-dark");
      }
    if (currentLayout === "light") {
    const lightBanner = document.querySelector("#light-banner-change");
    lightBanner?.classList?.add("active");
      $("body").removeClass("layout-dark");
      $("body").addClass("layout-light");
      }
    if (currentMenu === "side") {
        $("body").removeClass("top-menu");
        $("body").addClass("side-menu");
        }
    if (currentMenu === "top") {
        $("body").removeClass("side-menu");
        $("body").addClass("top-menu");
        }
}
    loadThemeFromLocal();
  /* Sidebar Change */
  const layoutChangeBtns = document.querySelectorAll("[data-layout]");
  // Get current layout from localStorage
  function changeLayout(e) {
    e.preventDefault();
    if (this.dataset.layout === "light") {
      localStorage.setItem("layout", "light");
      $('ul.l_sidebar li a,.l_sidebar a').removeClass('active');
      $(this).addClass("active");
      $("body").removeClass("layout-dark");
      $("body").addClass("layout-light");
    } else if (this.dataset.layout === "dark") {
      localStorage.setItem("layout", "dark");
      $('ul.l_sidebar li a,.l_sidebar a').removeClass('active');
      $(this).addClass("active");
      $("body").removeClass("layout-light");
      $("body").addClass("layout-dark");
    } else if (this.dataset.layout === "side") {
      localStorage.setItem("menu", "side");
      $('ul.l_navbar li a,.l_navbar a').removeClass('active');
      $(this).addClass("active");
      $("body").removeClass("top-menu");
      $("body").addClass("side-menu");
    } else if (this.dataset.layout === "top") {
      localStorage.setItem("menu", "top");
      $('ul.l_navbar li a,.l_navbar a').removeClass('active');
      $(this).addClass("active");
      $("body").removeClass("side-menu");
      $("body").addClass("top-menu");
    }
  }
  $('.enable-dark-mode').click(function () {
    $("body").toggleClass('layout-dark');
    $('.enable-dark-mode a').toggleClass('active');
  });

  if (layoutChangeBtns) {
    layoutChangeBtns.forEach((layoutChangeBtn) =>
      layoutChangeBtn.addEventListener("click", changeLayout)
    );
    layoutChangeBtns.forEach((layoutChangeBtn) =>
      layoutChangeBtn.addEventListener("click", closeCustomizer)
    );
  }
