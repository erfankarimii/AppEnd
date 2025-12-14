<template>
    <footer id="footer-section" class="footer section" style="display: block !important; visibility: visible !important; opacity: 1 !important; min-height: 200px !important;">
        <div class="container">
            <div class="row">
                <div class="col-md-16 mb-4">
                    <h5 class="mb-3">{{ translate("AppEnd") }}</h5>
                    <p class="text-muted">
                        {{ translate("A full-stack low-code platform for rapid application development.") }}
                    </p>
                </div>
                <div class="col-md-16 mb-4">
                    <h5 class="mb-3">{{ translate("Quick Links") }}</h5>
                    <ul class="list-unstyled">
                        <li class="mb-2">
                            <a href="#features-section" @click.prevent="scrollTo('features-section')" class="text-decoration-none">
                                {{ translate("Features") }}
                            </a>
                        </li>
                        <li class="mb-2">
                            <a href="#products-section" @click.prevent="scrollTo('products-section')" class="text-decoration-none">
                                {{ translate("Products") }}
                            </a>
                        </li>
                        <li class="mb-2">
                            <a href="#about-section" @click.prevent="scrollTo('about-section')" class="text-decoration-none">
                                {{ translate("About Us") }}
                            </a>
                        </li>
                        <li class="mb-2">
                            <a href="#pricing-section" @click.prevent="scrollTo('pricing-section')" class="text-decoration-none">
                                {{ translate("Pricing") }}
                            </a>
                        </li>
                        <li class="mb-2">
                            <a href="#contact-section" @click.prevent="scrollTo('contact-section')" class="text-decoration-none">
                                {{ translate("Contact Us") }}
                            </a>
                        </li>
                    </ul>
                </div>
                <div class="col-md-16 mb-4">
                    <h5 class="mb-3">{{ translate("Connect") }}</h5>
                    <div class="d-flex gap-3">
                        <a href="#" class="text-white text-decoration-none">
                            <i class="fa-brands fa-fw fa-github fa-2x"></i>
                        </a>
                        <a href="#" class="text-white text-decoration-none">
                            <i class="fa-brands fa-fw fa-twitter fa-2x"></i>
                        </a>
                        <a href="#" class="text-white text-decoration-none">
                            <i class="fa-brands fa-fw fa-linkedin fa-2x"></i>
                        </a>
                    </div>
                </div>
            </div>
            <hr class="my-4 border-secondary" />
            <div class="row">
                <div class="col-md-24 text-center">
                    <p class="mb-0 text-muted">
                        &copy; {{ new Date().getFullYear() }} {{ translate("AppEnd") }}. {{ translate("All rights reserved") }}.
                    </p>
                </div>
            </div>
        </div>
    </footer>
</template>

<script>
let _this = { cid: "", c: null };

export default {
    name: 'Footer',
    setup(props) { 
        try {
            _this.cid = props['cid'] || 'footerSection'; 
            console.log('Footer component setup called with cid:', _this.cid);
        } catch (e) {
            console.error('Error in Footer setup:', e);
        }
    },
    data() { 
        return _this; 
    },
    created() { 
        try {
            _this.c = this;
            console.log('Footer component created');
        } catch (e) {
            console.error('Error in Footer created:', e);
        }
    },
    mounted() { 
        console.log('Footer component mounted successfully');
        // Force footer to be visible
        this.$nextTick(() => {
            const footerEl = document.querySelector('#footer-section');
            if (footerEl) {
                footerEl.style.cssText = 'display: block !important; visibility: visible !important; opacity: 1 !important; position: relative !important; width: 100% !important;';
                console.log('Footer element found and made visible', footerEl);
            } else {
                console.error('Footer element not found in DOM after nextTick');
                // Try again after a delay
                setTimeout(() => {
                    const footerEl2 = document.querySelector('#footer-section');
                    if (footerEl2) {
                        footerEl2.style.cssText = 'display: block !important; visibility: visible !important; opacity: 1 !important; position: relative !important; width: 100% !important;';
                        console.log('Footer element found on retry');
                    } else {
                        console.error('Footer element still not found after retry');
                    }
                }, 1000);
            }
        });
    },
    methods: {
        translate(key) {
            if (typeof shared !== 'undefined' && typeof shared.translate === 'function') {
                return shared.translate(key);
            }
            return key;
        },
        scrollTo(sectionId) {
            const section = document.querySelector(`#${sectionId}`);
            if (section) {
                const offset = 80;
                const elementPosition = section.getBoundingClientRect().top;
                const offsetPosition = elementPosition + window.pageYOffset - offset;
                window.scrollTo({
                    top: offsetPosition,
                    behavior: 'smooth'
                });
            }
        }
    },
    props: { cid: String }
}
</script>

<style scoped>
</style>

